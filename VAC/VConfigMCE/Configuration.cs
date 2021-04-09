using System;
using System.Collections.Generic;
using System.Net;
using BepInEx;
using HarmonyLib;
using System.Reflection;
using BepInEx.Logging;
using UnityEngine;
using ModConfigEnforcer;

namespace VAC.VConfigMCE
{
    public class Configuration
    {
        public static void SetConfig()
        {
            // MCE - AntiMods
            VACPlugin.AntiMods_IsEnabled = ConfigManager.RegisterModConfigVariable<bool>(VACPlugin.pluginid, "IsEnabled", true, "AntiMods", "Activate/Disactivate this Section", false);
            VACPlugin.forcesamemods = ConfigManager.RegisterModConfigVariable<bool>(VACPlugin.pluginid, "ForceSameMods", false, "AntiMods", "Force the client and server to have the same mods.", false);
            VACPlugin.adminbypass = ConfigManager.RegisterModConfigVariable<bool>(VACPlugin.pluginid, "AdminBypass", false, "AntiMods", "Bypass administrators, in other words, ignore administrators.", false);
            
            // MCE - AntiParams
            VACPlugin.AntiParams_IsEnabled = ConfigManager.RegisterModConfigVariable<bool>(VACPlugin.pluginid, "Isnabled", true, "AntiParams", "Activate/Disactivate this Section", false);
            VACPlugin.ban_on_trigger = ConfigManager.RegisterModConfigVariable<bool>(VACPlugin.pluginid, "BanIfTrigger", false, "AntiParams", "If enabled, if a player breaks an Anti-Params rule he is banned and not kicked", false);
            VACPlugin.adminbypass = ConfigManager.RegisterModConfigVariable<bool>(VACPlugin.pluginid, "AdminBypass", false, "AntiParams", "Bypass administrators, in other words, ignore administrators.", false);
            VACPlugin.anti_fly = ConfigManager.RegisterModConfigVariable<bool>(VACPlugin.pluginid, "AntiFly", false, "AntiParams", "Activates the Anti-Fly function of the Anti-Params.", false);
            VACPlugin.anti_debug_mode = ConfigManager.RegisterModConfigVariable<bool>(VACPlugin.pluginid, "AntiDebug", false, "AntiParams", "Activates the Anti-Debug function of the Anti-Params.", false);
            VACPlugin.anti_god_mode = ConfigManager.RegisterModConfigVariable<bool>(VACPlugin.pluginid, "AntiGodMode", false, "AntiParams", "Activates the Anti-GodMode function of the Anti-Params.", false);
            VACPlugin.anti_damage_boost = ConfigManager.RegisterModConfigVariable<bool>(VACPlugin.pluginid, "AntiDamageBoost", false, "AntiParams", "Detects players who deal very high damage.", false);
            VACPlugin.anti_health_boost = ConfigManager.RegisterModConfigVariable<bool>(VACPlugin.pluginid, "AntiHealthBoost", false, "AntiParams", "[TODO!] Detects players with a very high life span.", false);
            
            // MCE - ServerConfig
            VACPlugin.debugmode = ConfigManager.RegisterModConfigVariable<bool>(VACPlugin.pluginid, "DebugMode", false, "Developer", "Activates the Debug Mode, debug messages will appear everywhere.", false);
            
            // MCE - MessagesConfig
            VACPlugin.AntiModsActivated = ConfigManager.RegisterModConfigVariable<string>(VACPlugin.pluginid, "AntiModsActivated", "Anti-Mods is Enabled, sucessfully activated!", "Messages", "Only one Message", false);
            
            VACPlugin.AntiModsError = ConfigManager.RegisterModConfigVariable<string>(VACPlugin.pluginid, "AntiModsError", "There was a glitch when generating the Anti-Mods hash! Anti-Mods is not working, check the console for errors and restart the application.", "Messages", "Only one Message", false);
            
            VACPlugin.AntiModsKickClient = ConfigManager.RegisterModConfigVariable<string>(VACPlugin.pluginid, "AntiModsKickClient", "[AntiMods]: You were kicked for using prohibited or outdated mods. Your data was sent to the server.", "Messages", "Only one Message", false);
            
            VACPlugin.AntiModsKickServer = ConfigManager.RegisterModConfigVariable<string>(VACPlugin.pluginid, "AntiModsKickServer", "[AntiMods]: You have been kicked for using Prohibited Mods.", "Messages", "Only one Message", false);
            
            VACPlugin.AntiParamsMsg = ConfigManager.RegisterModConfigVariable<string>(VACPlugin.pluginid, "AntiParamsMessage", "Player: {0} Banned for {1}", "Messages", "Only one Message", false);
        }
    }
}