:: Expands the default Arch qcow system image
@echo OFF
..\tools\7z\7z.exe x system/system.7z.001
echo system.vmdk expanded. Press any key to exit.
pause > nul 2>&1