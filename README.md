# WSA_Gapps_Tool

Simple tool for adding Google apps and services to WSA system images, without the need for WSL (or having to interact with a terminal).

Note: This tool **only** runs on Windows.

***This project is very much still in development, and requires a lot of work before a beta version can be released.***

## Minimum requirements
- Windows 11 (tool can be used on Windows 10 for generating MSIX package only)
- .NET Framework 4.7.2
- 8 GB of RAM (at least 10 - 12 GB is ideal, especially for WSA)
- At least 16 GB of free space on your system drive (a solid-state drive is ***highly*** recommended)
- Developer Mode turned on in Settings (required to allow apps outside of the Microsoft Store to be installed)

## How it works (In-depth)
Step 1: When the user clicks the Start button on the main window, the application will create a folder in the same location as itself named `cache`, and will use it to prepare the required files. First, it checks to see if it needs to download the latest Android 11 gapps package and/or the WSA MSIX application package (and does so if needed), then it begins getting everything ready.

Step 2: Once the application knows it has all the files it needs, it begins preparing them. It starts off by extracting the MSIX package to `cache/msix`, and collecting the included Android system images. It then creates a folder (`cache/temp`), and creates the following folders inside it: `images` | `gapps`. This directory will be used later to generate `data.vhdx` for use with the QEMU virtual machine.

Step 3: The application copies the following files: `system.img`, `system_ext.img`, `vendor.img` and `product.img` to `cache/temp/images`, followed by copying the gapps zip file to `cache/temp/gapps`.

Step 4: At this point, the application is done copying the files. The last step for preparation is to create an 8GB VHDX virtual disk image named `data.vhdx` in the `vm` folder at the root of the repository directory, followed by writing the contents of `cache/temp` to it. File preparation is now complete, and the virtual machine will now have everything it needs to successfully modify the Android system images.

Step 5: The application starts QEMU in headless mode as follows:
  - Give VM as many cores as the host CPU has
  - Allocate 1024 MB of RAM (or 2048 MB, depending on what's available)
  - Map virtual serial port to stdio (so that the application can receive and process messages from the VM)
  - Attach `vm/system.qcow2` as drive 0, with bootindex=0 and snapshot mode on, so any changes are not persistent
  - Attach `vm/data.vhdx` as drive 1 (will be /dev/sdb1 in VM)
  - Attach `vm/config.vhdx` as drive 2 (will be /dev/sdc1 in VM)

Once QEMU is started, Arch will begin booting. When it is finished booting, it will run `/home/arch/autostart.sh`. This script mounts the config drive (`/dev/sdc1`), and runs the scripts from it, starting with `run.sh` (the main script). When executed, `run.sh` initializes the serial port, and sends a ready message to tell the application that it is up and running. From this point on, the serial port is used to send status information back to the application.

The script then mounts the data drive (`/dev/sdb1`), and checks to see if the files it is expecting are there. Once it verifies that they are, it can begin. In a nutshell, the Android system images are mounted, the contents of the gapps package are copied to the images, the permissions are set as needed, and then the images are finalized and unmounted. The modification process is now complete. The VM sends the `complete` signal through the serial port, and then runs `cleanup.sh` (located alongside `run.sh`), which unmounts everything (to prevent data corruption), and shuts the virtual machine down.

Step 6: Once the VM shuts down, the application extracts the contents of `data.vhdx` and copies the new modified images to `cache/msix`, replacing the ones that were there before. It then moves the MSIX folder to `C:/`, renaming it `WSA`, and calls the PowerShell command `Add-AppxPackage` to "install" the package and make it usable.

## Building
**Requires Visual Studio 2022 Community or later.**
- **Step 1:** Open Visual Studio, and click "Clone a repository" under the "Get started" section on the start page. Alternatively, you can just [download the repository](https://github.com/JosephM101/WSA_Gapps_Tool/archive/refs/heads/main.zip). The solution is located in the `WsaGappsTool_src` folder.
- **Step 2:** When the solution is opened, right-click on the solution in `Solution Explorer`, and click `Restore NuGet packages`. Wait until that completes.
- **Step 3:** Build the solution. You can do this by either pressing <kbd>F6</kbd>, or by going to the top of the window and clicking Build -> Build Solution.
- **Step 4:** If the build was successful, there should be a new folder named `bin` at the root of the repository containing the application itself and its required files.

## Usage
- **Step 1:** [Download the repository as a zip file.](https://github.com/JosephM101/WSA_Gapps_Tool/archive/refs/heads/main.zip)

NOTE: Download uses ~900MB. Minimum free space required is 8 GB.

- **Step 2:** Open File Explorer, and navigate to the directory where you downloaded/extracted the repository. Open the `bin` directory at the repository root, and run `WsaGappsTool.exe`. If `bin` does not exist, you will need to build the solution. Follow the steps under [Building](#building).
- **Step 3:** When the application opens, you will be asked to specify the paths of the WSA MSIX package, and the gapps pico package. If you did not download these, leave the boxes empty. The application will automatically download these files when the process starts.
- **Step 4:** Click the Start button. The process will begin. You no longer need to interact with the application. Depending on the speed of your system, the process will take 20+ minutes.

**NOTE: If your machine does not meet all the hardware requirements as listed [above](#minimum-requirements), it's a good idea to close as many applications as you can to free up memory and CPU resources.

Once the tool is done, if you are running Windows 11, the tool will move everything it has done to `C:\WSA`, and will run `Add-AppxPackage` to register the AppxManifest.xml file in the package, and allow the app to be used.
