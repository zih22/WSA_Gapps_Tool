cd %~dp0
..\qemu\qemu-system-x86_64.exe -no-user-config -display sdl -net nic -net user -display sdl -M pc -smp cores=4 -m 4096 -device ich9-intel-hda -device ich9-ahci,id=sata -device ide-hd,bus=sata.2,drive=ARCH,bootindex=0 -drive id=ARCH,if=none,file=system.qcow2,format=qcow2 -device ide-hd,bus=sata.3,drive=DATA -drive id=DATA,if=none,file=data.vhdx,format=vhdx -device ide-hd,bus=sata.4,drive=CONFIG -drive id=CONFIG,if=none,file=config.vhdx,format=vhdx