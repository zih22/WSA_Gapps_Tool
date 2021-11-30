@echo off

:: Check for admin rights
net session >nul 2>&1
if %errorLevel% == 0 (
	echo Starting...
) else (
	echo This script requires administrator rights to run.
	echo Press any key to exit...
	pause > nul
	exit
)

set vhdFilepath = c:\data.vhdx
set vhdLabel = data

:: Log current directory
set currentDir = %cd%

:: Move to script directory if we're not already there
cd %~dp0 >nul 2>&1

:: Check if VHD exists and/or is attached
if exist c:\data.vhdx (
	(echo select vdisk file=c:\data.vhdx
	echo detach vdisk
	echo exit) | diskpart
	rm -f c:\data.vhdx
)

echo Creating VHDX...
:: Pass commands to DISKPART to make 8GB data drive for holding the .img files
(echo create vdisk file=c:\data.vhdx maximum=8192 type=expandable
echo select vdisk file=c:\data.vhdx
echo attach vdisk
echo detail vdisk
echo convert mbr
echo create partition primary
echo format fs=fat32 label=wsa_images quick
echo assign
echo detach vdisk
echo exit) | diskpart > nul 2>&1

if %errorLevel% == 0 (
	echo Done.
) else (
	echo Error creating VHDX.
	echo Press any key to exit...
	pause > nul
	exit
)

echo Moving VHDX...
:: Move VHDX to current directory
move c:\data.vhdx .

echo Correcting permissions...
icacls data.vhdx /grant "Authenticated Users":(OI)(CI)M
icacls data.vhdx /grant "everyone":(OI)(CI)M
icacls data.vhdx /grant "domain\user":(OI)(CI)M

echo Done!
echo Double-click the VHDX file to mount it, then copy the WSA image files over to the mounted drive (should be called WSA_IMAGES). When you're done, right-click the mounted drive and click "Eject".

cd %currentDir% > nul 2>&1

pause