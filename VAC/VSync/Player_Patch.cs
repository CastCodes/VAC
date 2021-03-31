/*using UnityEngine;
using HarmonyLib;
using VAC.VConfig;

namespace VAC.VSync
{
    class VersionInfo
    {
        /// <summary>
        /// Get version string and enforce mod if enabled
        /// </summary>
        [HarmonyPatch(typeof(Version), "GetVersionString")]
        public static class Version_GetVersionString_Patch
        {
            private static void Postfix(ref string __result)
            {
                Debug.Log($"Version Generator started.");
                if (Configuration.Current.Server.IsEnabled)
                {
                    if (Configuration.Current.Server.enforceMod)
                    {
                        __result = __result + "@" + VACPlugin.version;
                        Debug.Log($"Version generated with the mod : {__result}");
                    }
                }
                else
                {
                    Debug.Log($"Generated version : {__result}");
                }
            }
        }
    }
}*/