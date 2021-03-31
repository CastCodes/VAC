xcopy "C:\Users\TheWh\Documents\GitHub\VAC\VAC\bin\Debug\VAC.dll" "C:\Users\TheWh\Documents\Valheim SteamCMD\steamapps\common\Valheim dedicated server\BepInEx\plugins" /K /D /H /Y
xcopy "C:\Users\TheWh\Documents\GitHub\VAC\VAC\bin\Debug\VAC.dll" "E:\Games\SteamLibrary\steamapps\common\Valheim\BepInEx\plugins" /K /D /H /Y
start /d "C:\Users\TheWh\Documents\Valheim SteamCMD\steamapps\common\Valheim dedicated server" start_headless_server.bat
start /d "E:\Games\SteamLibrary\steamapps\common\Valheim" valheim.exe