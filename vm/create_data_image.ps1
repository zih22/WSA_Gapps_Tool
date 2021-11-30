function main() {
    New-VHD -Path "data.vhdx" -Dynamic -SizeBytes 8GB | Mount-VHD -Passthru |Initialize-Disk -Passthru |New-Partition -AssignDriveLetter -UseMaximumSize |Format-Volume -FileSystem FAT32 -NewFileSystemLabel "WSA_IMAGES" -Confirm:$false -Force
}

# Check if Hyper-V feature is enabled
$hyperv = Get-WindowsOptionalFeature -FeatureName Microsoft-Hyper-V-Management-PowerShell -Online
if($hyperv.State -eq "Enabled") {
    main
} else {
    Enable-WindowsOptionalFeature -Online -FeatureName Microsoft-Hyper-V-Management-PowerShell
}