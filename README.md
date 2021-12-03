# WSA_Gapps_Tool

Simple tool for adding Google apps and services to WSA system images, without the need for WSL (or having to interact with a terminal).

## How it works (In-depth)
Step 1: When the user clicks the Start button on the main window, the application will create a folder in the same location as itself named `cache`, and will use it to prepare the required files. First, it checks to see if it needs to download the latest Android 11 gapps package and/or the WSA MSIX application package (and does so if needed), then it begins getting everything ready.
Step 2: Once the application knows it has all the files it needs, it begins preparing them. It starts off by extracting the MSIX package to `cache/msix`, and collecting the included Android system images. It then creates a folder (`cache/temp`), and creates the following folders inside it: `images` | `gapps`. This directory will be used later to generate `data.vhdx` for use with the QEMU virtual machine.
Step 3: The application copies the following files: `system.img`, `system_ext.img`, `vendor.img` and `product.img` to `cache/temp/images`, followed by copying the gapps zip file to `cache/temp/gapps`.
Step 4: At this point, the application is done copying the files. The last step for preparation is to create an 8GB VHDX virtual disk image named `data.vhdx` in the `vm` folder at the root of the repository directory, followed by writing the contents of `cache/temp` to it. File preparation is now complete, and the virtual machine will now have everything it needs to successfully modify the Android system images.
