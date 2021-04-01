using HarmonyLib;

namespace VAC.VSync
{
    /*/// <summary>
    /// Adding VAC logo and version text
    /// </summary>
    [HarmonyPatch(typeof(FejdStartup), "SetupGui")]
    public static class FejdStartup_SetupGui_Patch
    {
        private static void Postfix(ref FejdStartup __instance)
        {
            // version text for bottom right of startup
            __instance.m_versionLabel.fontSize = 14;
            string gameVersion = Version.CombineVersion(global::Version.m_major, global::Version.m_minor, global::Version.m_patch);
            __instance.m_versionLabel.text = "version " + gameVersion + "\n" + "VAC " + VACPlugin.version;
        }
    }*/

    /// <summary>
    /// Show Custom Connection Errors
    /// </summary>
    [HarmonyPatch(typeof (FejdStartup))]
    public class FejdStartup_ShowConnectErrorPatch
    {
        [HarmonyPatch(typeof (FejdStartup), "ShowConnectError")]
        public static void Postfix(FejdStartup __instance)
        {
            if (ZNet.GetConnectionStatus() != (ZNet.ConnectionStatus) 99)
                return;
            __instance.m_connectionFailedError.text = "[AntiMods]: You were kicked for using prohibited or outdated mods.\nYour data was sent to the server.";
            ZLog.LogError("Player Found with Cheats in Folder or Running.");
            ZLog.LogError("Sending player information to the server...");
            ZLog.LogError("Player disconnected from server for using improper programs...");
        }
    }
}