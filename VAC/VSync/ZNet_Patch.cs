using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using VConfig;
using VAC.VTools;

// ToDo add packet system to convey map markers
namespace VAC.VSync
{
    [HarmonyPatch(typeof(ZNet))]
    public class HookZNet
    {
        /// <summary>
        /// Hook base GetOtherPublicPlayer method
        /// </summary>
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(ZNet), "GetOtherPublicPlayers", new Type[] {typeof(List<ZNet.PlayerInfo>)})]
        public static void GetOtherPublicPlayers(object instance, List<ZNet.PlayerInfo> playerList)
        {
            ZLog.LogWarning("Don't worry, this is a Reverse Patch running a NotImplementedException(), just wait...");
            throw new NotImplementedException();
        } 
    }

    /// <summary>
    /// Send queued RPCs
    /// </summary>
    [HarmonyPatch(typeof(ZNet), "SendPeriodicData")]
    public static class PeriodicDataHandler
    {
        private static void Postfix()
        {
            RpcQueue.SendNextRpc();
        }
    }

    /// <summary>
    /// Sync server client configuration
    /// </summary>
    [HarmonyPatch(typeof(ZNet), "RPC_PeerInfo")]
    public static class ConfigServerSync
    {
        private static void Postfix(ref ZNet __instance)
        {
            if (!ZNet.m_isServer)
            {
                ZLog.LogWarning("The Valheim Anti Cheat is checking the Client Hash...");
                ZLog.Log("-------------------- VAC-SYNC REQUEST SENT");
                ZRoutedRpc.instance.InvokeRoutedRPC(ZRoutedRpc.instance.GetServerPeerID(), "VACConfigSync", new object[] { new ZPackage() });
            }
        }
    }

    /// <summary>
    /// Load settngs from server instance
    /// </summary>
    [HarmonyPatch(typeof(ZNet), "Shutdown")]
    public static class OnErrorLoadOwnIni
    {
        private static void Prefix(ref ZNet __instance)
        {
            if (!__instance.IsServer())
            {
                VACPlugin.harmony.UnpatchSelf();

                // Load the client config file on server ZNet instance exit (server disconnect)
                if (Extra.LoadSettings() != true)
                {
                    Debug.LogError("Error when loading the configuration file.");
                }

                VACPlugin.harmony.PatchAll();
            }
        }
    }
}
