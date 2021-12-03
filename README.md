# WSA_Gapps_Tool

Simple tool for adding Google apps and services to WSA system images, without the need for WSL (or having to interact with a terminal).

## How it works (In-depth)
Step 1: When the user clicks the Start button on the main window, the application will create a folder in the same location as itself named `cache`, and will use it to prepare the required files. First, it checks to see if it needs to download the latest Android 11 gapps package and/or the WSA MSIX application package (and does so if needed), then it begins getting everything ready.

Step 2: Once the application knows it has all the files it needs, it begins preparing them. It starts off by extracting the MSIX package to `cache/msix`, and collecting the included Android system images. It then creates a folder (`cache/temp`), and creates the following folders inside it: `images` | `gapps`. This directory will be used later to generate `data.vhdx` for use with the QEMU virtual machine.

Step 3: The application copies the following files: `system.img`, `system_ext.img`, `vendor.img` and `product.img` to `cache/temp/images`, followed by copying the gapps zip file to `cache/temp/gapps`.

Step 4: At this point, the application is done copying the files. The last step for preparation is to create an 8GB VHDX virtual disk image named `data.vhdx` in the `vm` folder at the root of the repository directory, followed by writing the contents of `cache/temp` to it. File preparation is now complete, and the virtual machine will now have everything it needs to successfully modify the Android system images.

Step 5: The application starts QEMU in headless mode as follows:
  - Give VM as many cores as the host CPU has
  - Allocate 1024 MB of RAM (or 2048 MB, depending on what's available)
  - Map virtual serial port to stdio (so that the application can receive and process messages from the VM)
  - Attach `vm/system.qcow2` as drive 0, with bootindex=0 and snapshot mode on
  - Attach `vm/data.vhdx` as drive 1 (will be /dev/sdb1 in VM)
  - Attach `vm/config.vhdx` as drive 2 (will be /dev/sdc1 in VM)

Once QEMU is started, Arch will begin booting. When it is finished booting, it will run `/home/arch/autostart.sh`. This script mounts the config drive (`/dev/sdc1`), and runs the scripts from it, starting with `run.sh` (the main script). When executed, `run.sh` initializes the serial port, and sends a ready message to tell the application that it is up and running. From this point on, the serial port is used to send status information back to the application.

The script then mounts the data drive (`/dev/sdb1`), and checks to see if the files it is expecting are there. Once it verifies that they are, it can begin. In a nutshell, the Android system images are mounted, the contents of the gapps package are copied to the images, the permissions are set as needed, and then the images are finalized and unmounted. The modification process is now complete. The VM sends the `complete` signal through the serial port, and then runs `cleanup.sh` (located alongside `run.sh`), which unmounts everything (to prevent data corruption), and shuts the virtual machine down.

