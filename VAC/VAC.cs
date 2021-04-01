﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using BepInEx;
using HarmonyLib;
using System.Reflection;
using BepInEx.Logging;
using BepInEx.Configuration;
using UnityEngine;


namespace VAC
{
    // Copyright ©  2021 JOÃO PEDRO VIANA FREITAS // https://linktr.ee/JoaoPVF
    // GITHUB REPOSITORY https://github.com/CastCodes/VAC

    [BepInPlugin("br.com.castcodes.vac", "Valheim Anti-Cheat", version)]
    public class VACPlugin : BaseUnityPlugin
    {
        // Basic Project Info
        public const string version = "0.0.5";
        public static string description = "An AntiCheat to Valheim";
        public static string copyright = "Copyright © - Joao Pster";
        public static string newestVersion = "";

        // Project Repository Info
        public static string Repository = "https://github.com/CastCodes/VAC";
        public static string ApiRepository = "https://api.github.com/repos/CastCodes/VAC/tags";

        // Hash Variables
        public const int HashLength = 32;
        public static string PluginsHash = "vlanch";

        // Anti-Cheat Variables
        public static Harmony harmony = new Harmony("br.com.castcodes.vac");
        public static List<ZNetPeer> toKick = new List<ZNetPeer>();
        public static Dictionary<ZNetPeer, Vector3> posMap;
        public static ManualLogSource logger;

        void Awake()
        {
            // ========================= ANTI-CHEAT
            
            logger = Logger;
            posMap = new Dictionary<ZNetPeer, Vector3>();
            toKick = new List<ZNetPeer>();
            
            // ========================= MAIN-PLUGIN
            Logger.LogInfo("Trying to load the configuration file");
            if (VConfig.Extra.LoadSettings() != true)
            {
                Logger.LogError("Error when loading the configuration file.");
            }
            else
            {
                Logger.LogInfo("Configuration file loaded successfully.");

                harmony.PatchAll();
                ZLog.LogWarning("Sync Configs is: " + VConfig.Configuration.Current.Server.IsEnabled);
                //ZLog.LogWarning("Enforce Mod is: " + VConfig.Configuration.Current.Server.enforceMod);
                
                string newerversionused = IsNewVersionAvailable();
                if (newerversionused == "new")
                {
                    Logger.LogError("There is a newer version available of Valheim Anti Cheat.");
                    Logger.LogWarning("Please visit " + VACPlugin.Repository + ".");
                }
                else if (newerversionused == "same")
                {
                    Logger.LogInfo("ValheimPlus [" + version + "] is up to date.");
                }
                else if (newerversionused == "old")
                {
                    Logger.LogError("You are in a version ahead of the most current one.");
                    Logger.LogWarning(
                        "If you are not a developer, please switch back to the most current stable version published.");
                    Logger.LogWarning("Please visit " + VACPlugin.Repository + ".");
                }
                else if (newerversionused == "fail")
                {
                    Logger.LogError("There was a fail in stipulating the version.");
                    Logger.LogWarning("Please visit " + VACPlugin.Repository + ".");
                }
                else
                {
                    Logger.LogError("There was a fail in stipulating the version.");
                    Logger.LogWarning("Please visit " + VACPlugin.Repository + ".");
                } 
            }

            // By RaIŇİ᎐#0213 ^^
            AntiMods();
        }

        #region AntiMods
        public static void AntiMods()
        {
            PluginsHash = VAC.AntiMods.HashAlgorithmExtensions.CreateMd5ForFolder(Paths.PluginPath);
            ZLog.Log((object) ("[AntiMods]: Computed hash: " + PluginsHash));
            if (Paths.ProcessName.Equals("valheim_server", StringComparison.OrdinalIgnoreCase))
            {


                MethodInfo methodInfo = AccessTools.Method(typeof(ZNet), "RPC_PeerInfo", new System.Type[2]
                {
                    typeof(ZRpc),
                    typeof(ZPackage)
                });

                if ((object) methodInfo == null)
                {
                    ZLog.LogError((object) "[AntiMods] Could not find ZNet:RPC_PeerInfo");
                    return;
                }

                harmony.Patch((MethodBase) methodInfo, new HarmonyMethod(AccessTools.Method(
                    typeof(AntiMods.GamePatches.ZNet_RPC_PeerInfoPatch), "Prefix", new System.Type[3]
                    {
                        typeof(ZNet).MakeByRefType(),
                        typeof(ZRpc),
                        typeof(ZPackage)
                    })));

                ZLog.Log((object) "[AntiMods] Patched server!");
            }
            else
            {
                MethodInfo methodInfo1 = AccessTools.Method(typeof(ZNet), "RPC_PeerInfo", new System.Type[2]
                {
                    typeof(ZRpc),
                    typeof(ZPackage)
                });
                if ((object) methodInfo1 == null)
                {
                    ZLog.LogError((object) "[AntiMods] Could not find ZNet:RPC_PeerInfo");
                    return;
                }

                MethodInfo methodInfo2 = AccessTools.Method(typeof(ZNet), "SendPeerInfo", new System.Type[2]
                {
                    typeof(ZRpc),
                    typeof(string)
                });
                if ((object) methodInfo2 == null)
                {
                    ZLog.LogError((object) "[AntiMods] Could not find ZNet:SendPeerInfo");
                    return;
                }

                harmony.Patch((MethodBase) methodInfo2, transpiler: new HarmonyMethod(AccessTools.Method(
                    typeof(AntiMods.ILPatches), "SendPeerInfo_Transpile", new System.Type[1]
                    {
                        typeof(IEnumerable<CodeInstruction>)
                    })));
                harmony.Patch((MethodBase) methodInfo1,
                    postfix: new HarmonyMethod(
                        AccessTools.Method(typeof(AntiMods.GamePatches.ZNet_RPC_PeerInfoPatch), "Postfix")));
                ZLog.Log((object) "[AntiMods] Patched client!");
            }

            if (PluginsHash != "")
            {
                ZLog.LogWarning((object) "Anti-Mods is Enabled, sucessfully activated!");
            }
            else
            {
                ZLog.LogError("There was a glitch when generating the Anti-Mods hash! Anti-Mods is not working, check the console for errors and restart the application.");
            }
        }
        #endregion
        
        
        // Functions

        #region IsNewVersion
        public static string IsNewVersionAvailable()
        {
            WebClient client = new WebClient();

            client.Headers.Add("User-Agent: VAC");

            try
            {
                string reply = client.DownloadString(ApiRepository);
                newestVersion = reply.Split(new[] {","}, StringSplitOptions.None)[0].Trim().Replace("\"", "")
                    .Replace("[{name:", "");
            }
            catch
            {
                ZLog.Log("The newest version could not be determined.");
                newestVersion = "Unknown";
            }

            //Parse versions for proper version check
            if (System.Version.TryParse(newestVersion, out System.Version newVersion))
            {
                if (System.Version.TryParse(version, out System.Version currentVersion))
                {
                    if (currentVersion < newVersion && (currentVersion != newVersion))
                    {
                        return "new";
                    }

                    if (currentVersion > newVersion && (currentVersion != newVersion))
                    {
                        return "old";
                    }

                    if (currentVersion == newVersion)
                    {
                        return "same";
                    }
                }
            }
            else //Fallback version check if the version parsing fails
            {
                if (newestVersion != version)
                {
                    return "fail";
                }
            }

            return "fail";
        }
        #endregion
    }
}