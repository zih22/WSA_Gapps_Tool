@echo OFF

:: Delete folder bin/cache
del /q /f /s %~dp0\bin\cache\*

:: Delete file vm/data.vhdx
del /q /f /s %~dp0\vm\data.vhdx