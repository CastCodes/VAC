# VAC
Valheim Anti Cheat - Developing...

Discord: https://discord.gg/sgWBggXSWJ

# How to Install
1. Install [BepInExPack Valheim](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/)
3. Download latest version [here](https://github.com/CastCodes/VAC/releases).
4. Extract the file you downloaded into the folder: [GameFolder]/BepInEx
5. The mod must be on both client and server

(Inside the [GameFolder]/BepInEx/config folder will be the VAC configuration file.)

# Current Features
## Config
* Automatic configuration synchronization between the server and the client.
* You can enable or disable various VAC features.

## Anti-Params
Detection of abnormal game parameters or **debug** modes.

* Health (TODO)
* Fly and Speed (TODO)
* Debug Modes
* Damage to: Chars, Builds, Resources, Destructibles

## Anti-Mods
Compare the BepInEx/plugins folder of the server with the client, if something is different, like: 
* Different mods
* Missing mods
* More mods than the server

In case something is wrong, it will kick the player out when he tries to log in.

# Credits
### Dev Credits
* João Pster (https://github.com/J-Pster)
* Max Bridgland (https://github.com/M4cs)

### Repository Credits
* Valheim+ Project (https://github.com/valheimPlus) - Config Sync
