# VM configuration

*NOTE: All VHDX images are formatted as FAT32*

## data.vhdx
If you're setting up the data.vhdx drive yourself, there are a couple things to note:
- The main script uses, and expects, a specific folder structure on the virtual drive. There are two folders at the root of the filesystem, and they are as follows: "img" and "gapps". The folders must have these exact names for the script to work properly, and it is recommended for them to contain only their intended data. This can be done automatically by running the `expand_data_image.cmd` script in the `vm` directory.

## config.vhdx
This virtual disk houses the configuration for the VM, such as the script that builds the new WSA system images (run.sh). **It is recommended that you do not touch this disk or modify its contents unless you know what you're doing.**

## system.qcow2
This is the system drive that houses a minimal Arch installation with only the required tools installed. If it is not in the `vm` folder, you need to run the `expand-system-image.cmd` script. This will extract the `qcow2` image from the compressed split archive files found in the `system` folder. By default, QEMU is configured to attach the disk in snapshot mode, so the contents of the disk do not get modified. However, if they do, you can delete the disk image, and re-run `expand-system-image.cmd` to get a fresh image.
