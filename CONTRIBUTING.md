# Contributing to VAC

## How to setup your environment for VAC development
How to setup the development enviroment to compile ValheimPlus yourself.

1. Download the [BepInEx for Valheim package](https://valheim.thunderstore.io/package/download/denikson/BepInExPack_Valheim/5.4.701/).
   - Extract zip contents and copy the contents inside `/BepInExPack_Valheim/` and paste them in your Valheim root folder and overwrite every file when asked.
   - This package sets up your Valheim game with BepInEx configurations specifically for mod devs. Created by [BepInEx](https://github.com/BepInEx).
1. Copy over all the DLLs from Valheim/unstripped_corlib to Valheim/valheim_Data/Managed *(overwrite when asked)*
1. Download the [AssemblyPublicizer package](https://mega.nz/file/oQxEjCJI#_XPXEjwLfv9zpcF2HRakYzepMwaUXflA9txxhx4tACA).
   - This package has a tool that you'll use to create publicized versions of the `assembly_*.dll` files for your local development.
   - Repo: https://github.com/MrPurple6411/Bepinex-Tools/releases/tag/1.0.0-Publicizer by [MrPurple6411](https://github.com/MrPurple6411).
1. Drag and drop all `assembly_*.dll` files from "\Valheim\valheim_Data\Managed\" folder onto "AssemblyPublicizer.exe". This will create a new folder called "/publicized_assemblies/".
1. Define Enviroment Variable `VALHEIM_INSTALL` with path to Valheim Install Directory  
   - example: `setx VALHEIM_INSTALL "C:\Program Files\Steam\steamapps\common\Valheim" /M`

## Debugging with dnSpy

Thanks to mono and unity-mono being open source, we patched and compiled our own mono runtime and enabled actual live debugging of the game and the mod itself with dnSpy.

1. Download [dnSpy-net-win64](https://github.com/dnSpy/dnSpy/releases) and extract the exe.
2. Load all assemblies from \<Valheim>\unstripped_corlib into dnSpy (just drag&drop the folder onto it).
3. Load all assembly_* from \<Valheim>\valheim_Data\Managed into dnSpy (*do not load the publicized ones, they will not be loaded into the process and therefore can not be debugged*).
4. Load ValheimPlus.dll from \<Valheim>\BepInEx\plugins into dnSpy.
5. Copy .\libraries\Debug\mono-2.0-bdwgc.dll from this repo to \<Valheim>\MonoBleedingEdge\EmbedRuntime and overwrite the existing file.
6. Now go to `Debug` -> `Start Debugging` and select Unity debug engine. Select your valheim.exe as the executable and hit OK.
7. If you did set some breakpoints, the game will halt when it hits the breakpoint in memory and dnSpy will show you the objects in memory and lets you do much more useful stuff.

## Making a Pull Request
1. Only make a pull request for finished work. Otherwise, if we pull the work down to test it and it doesn't work, we don't know if it's because it's unfinished or if there's an unintentional bug.
   - If you'd like a review on your work before something it's finished, send us a link to a compare via Discord or make a "Draft" PR.
   - Discord: https://discord.gg/sgWBggXSWJ
2. If you want credit, add your credit to the `README.md` in your pull request if the work is more than a documentation update. We will not be able to track this ourselves and rely on you to add your preferred way of being credited.
