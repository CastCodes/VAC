using System;
using System.Collections.Generic;
using System.Net;
using BepInEx;
using HarmonyLib;
using System.Reflection;
using BepInEx.Logging;
using UnityEngine;
using ModConfigEnforcer;

namespace VAC
{
    // Copyright ©  2021 JOÃO PEDRO VIANA FREITAS // https://castcodes.com.br/
    // GITHUB REPOSITORY https://github.com/CastCodes/VAC

    [BepInPlugin(pluginid, pluginname, version)]
    [BepInDependency("pfhoenix.modconfigenforcer")]
    public class VACPlugin : BaseUnityPlugin
    {
        #region Basic_Infos_And_Variables
        
        // Basic Project Info
        public const string version = "1.0.1";
        public const string pluginname = "Valheim Anti-Cheat";
        public const string pluginid = "br.com.castcodes.vac";
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
        
        #endregion

        #region MCE
        
        // MCE - AntiMods
        public static ConfigVariable<bool> AntiMods_IsEnabled;
        public static ConfigVariable<bool> forcesamemods;
        public static ConfigVariable<bool> adminbypass;
        
        // MCE - AntiParams
        public static ConfigVariable<bool> AntiParams_IsEnabled;
        public static ConfigVariable<bool> ban_on_trigger;
        public static ConfigVariable<bool> admins_bypass;
        public static ConfigVariable<bool> anti_fly;
        public static ConfigVariable<bool> anti_debug_mode;
        public static ConfigVariable<bool> anti_god_mode;
        public static ConfigVariable<bool> anti_damage_boost;
        public static ConfigVariable<bool> anti_health_boost;
        
        // MCE - ServerConfig
        public static ConfigVariable<bool> debugmode;
        
        // MCE - MessagesConfig
        public static ConfigVariable<string> AntiModsActivated;
        public static ConfigVariable<string> AntiModsError;
        public static ConfigVariable<string> AntiModsKickServer;
        public static ConfigVariable<string> AntiModsKickClient;
        public static ConfigVariable<string> AntiParamsMsg;
        
        #endregion

        void Awake()
        {
            
            ConfigManager.ServerConfigReceived
            // MCE
            ConfigManager.RegisterMod(pluginid, Config);
            VConfigMCE.Configuration.SetConfig();
            harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "br.com.castcodes.vac");
            ZLog.LogWarning("===================== Configuration synchronization initiated by MCE!");

                // ========================= ANTI-CHEAT
            
            logger = Logger;
            posMap = new Dictionary<ZNetPeer, Vector3>();
            toKick = new List<ZNetPeer>();

            string newerversionused = IsNewVersionAvailable();
            if (newerversionused == "new")
            {
                Logger.LogError("There is a newer version available of Valheim Anti Cheat.");
                Logger.LogWarning("Please visit " + VACPlugin.Repository + ".");
            }
            else if (newerversionused == "same")
            {
                Logger.LogInfo("AntiMods [" + version + "] is up to date.");
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
                ZLog.LogWarning((object) VACPlugin.AntiModsActivated.Value);
            }
            else
            {
                ZLog.LogError(VACPlugin.AntiModsError.Value);
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
        
        private void OnDestroy()
        {
            if (harmony != null) harmony.UnpatchAll("br.com.castcodes.vac");
        }
    }
}