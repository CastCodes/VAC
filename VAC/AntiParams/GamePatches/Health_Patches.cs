using System.Net.PeerToPeer.Collaboration;
using HarmonyLib;

namespace VAC.AntiParams.GamePatches
{
    /*[HarmonyPatch(typeof (Character), "RPC_Heal")]
    public class HealthRPC_Heal_Patch
    {
        private static bool Prefix(ref Character __instance, ref long sender,ref float hp,ref bool showText)
        {
            float playerhealth = __instance.GetHealth();
            ZLog.LogWarning("Player Health:" + playerhealth);
            if (((double) playerhealth <= 0.0 || __instance.IsDead()) || !__instance.IsPlayer())
                return true;
            
            ZLog.LogWarning("Is Player/Is Dead:" + __instance.IsPlayer() + __instance.IsDead());
            
            float playermaxhealth = __instance.GetMaxHealth();
            Character toplayerHealth = __instance;
            ZLog.LogWarning("Max Health/Player Character:" + playermaxhealth + toplayerHealth);
            
            if (playermaxhealth >= 300f || playerhealth >= 400f)
            {
                ZNetPeer peer = ZNet.instance.GetPeer(toplayerHealth.GetInstanceID());
                ZLog.LogWarning("Peer:" + peer);
                if( peer != null && ( !ValheimPlusPlugin.admins_bypass || !ZNet.instance.m_adminList.Contains(peer.m_rpc.GetSocket().GetHostName()) ))
                {
                    ZLog.LogError("Jogador Detectado com Health Boost.");
                    //ValheimPlusPlugin.toKick.Add(peer);
                }  
                return true;
            }
            
            return true;
        }
    }*/
}