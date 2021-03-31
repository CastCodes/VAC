using System;
using HarmonyLib;

namespace VAC.VSync
{
    /// <summary>
    /// Sync server config to clients
    /// </summary>
    [HarmonyPatch(typeof(Game), "Start")]
    public static class GameStartPatch
    {
        private static void Prefix()
        {
            ZRoutedRpc.instance.Register("VACConfigSync", new Action<long, ZPackage>(VACSyncConfig.RPC_VACConfigSync)); //Config Sync
        }
    }
}