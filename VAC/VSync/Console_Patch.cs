using HarmonyLib;

namespace VAC.VSync
{
    /// <summary>
    /// Adding version data to console
    /// </summary>
    [HarmonyPatch(typeof(Console), "Awake")]
    public static class Console_Awake_Patch
    {
        private static void Postfix(ref Console __instance)
        {
            __instance.AddString("VAC [" + VACPlugin.version + "] is loaded.");
            if ((VACPlugin.IsNewVersionAvailable() == "new") && VACPlugin.newestVersion != "Unknown")
            {
                __instance.AddString("VAC [" + VACPlugin.version + "] is outdated, version [" + VACPlugin.newestVersion + "] is available.");
                __instance.AddString("Please visit " + VACPlugin.Repository + ".");
            }
            else
            {
                __instance.AddString("VAC [" + VACPlugin.version + "] is up to date.");
            }

            __instance.AddString("");
        }
    }
}