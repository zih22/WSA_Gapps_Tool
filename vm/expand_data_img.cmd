:: Expands the template vhdx image containing two folders for gapps and system images
@echo OFF
..\tools\7z\7z.exe x data/data.7z.001
echo data.vmdk expanded. Press any key to exit.
pause > nul 2>&1