using HarmonyLib;
using UnityEngine;
using VConfig;
using System;
using UnityEngine.Rendering;

namespace VAC.AntiParams.GamePatches
{
    [HarmonyPatch(typeof (ZNet), "OnNewConnection")]
    public class AddPlayer_Patch
    {
        public static void Postfix(ZNet __instance, ZNetPeer peer)
        {
            if (Configuration.Current.AntiParams.IsEnabled)
            {
                if (VACPlugin.posMap.ContainsKey(peer))
                    return;
                VACPlugin.posMap.Add(peer, Vector3.zero);
            }
        }
    }
    
    [HarmonyPatch(typeof (ZNet), "UpdatePlayerList")]
  public class UpdatePlayerList_Patch
  {
    private static void Postfix()
    {
      if (Configuration.Current.AntiParams.IsEnabled)
      {
        if (!((UnityEngine.Object) ZNet.instance != (UnityEngine.Object) null))
          return;
        if (ZNet.instance.IsServer() && SystemInfo.graphicsDeviceType != GraphicsDeviceType.Null &&
            (Player.m_players != null && Player.m_players.Count > 0))
        {
          foreach (Player player in Player.m_players)
          {
            ZNetPeer peerByPlayerName = ZNet.instance.GetPeerByPlayerName(player.name);
            if ((player.m_debugFly || player.m_noPlacementCost && Configuration.Current.AntiParams.anti_debug_mode) &&
                (peerByPlayerName != null &&
                 !ZNet.instance.m_adminList.Contains(peerByPlayerName.m_rpc.GetSocket().GetHostName())))
              VACPlugin.toKick.Add(peerByPlayerName);
            if (player.m_godMode && Configuration.Current.AntiParams.anti_god_mode && (peerByPlayerName != null &&
              !ZNet.instance.m_adminList.Contains(peerByPlayerName.m_rpc.GetSocket().GetHostName())))
              VACPlugin.toKick.Add(peerByPlayerName);
          }
        }

        if (ZNet.instance.m_peers.Count > 0)
        {
          foreach (ZNetPeer peer in ZNet.instance.m_peers)
          {
            if (VACPlugin.posMap[peer] == Vector3.zero)
              VACPlugin.posMap[peer] = peer.m_refPos;
            else if (Configuration.Current.AntiParams.anti_fly)
            {
              if ((double) Math.Abs(peer.m_refPos.x - VACPlugin.posMap[peer].x) > 70.0 ||
                  (double) Math.Abs(peer.m_refPos.y - VACPlugin.posMap[peer].y) > 35.0 ||
                  (double) Math.Abs(peer.m_refPos.y - VACPlugin.posMap[peer].y) > 15.0)
                VACPlugin.toKick.Add(peer);
              else
                VACPlugin.posMap[peer] = peer.m_refPos;
            }

            if (peer.IsReady() && !peer.m_characterID.IsNone() &&
                (ZNet.instance.m_zdoMan.GetZDO(peer.m_characterID).GetBool("DebugFly") &&
                 !ZNet.instance.m_adminList.Contains(peer.m_rpc.GetSocket().GetHostName())))
              VACPlugin.toKick.Add(peer);
          }
        }

        if (VACPlugin.toKick.Count <= 0)
          return;
        foreach (ZNetPeer znetPeer in VACPlugin.toKick)
        {
          if (!Configuration.Current.AntiParams.admins_bypass ||
              !ZNet.instance.m_adminList.Contains(znetPeer.m_rpc.GetSocket().GetHostName()))
          {
            if (Configuration.Current.AntiParams.ban_on_trigger)
            {
              ZNet.instance.Ban(znetPeer.m_playerName);
              ZLog.LogError("Jogador: " + znetPeer.m_playerName + znetPeer.m_uid + znetPeer.m_characterID +
                            " Banido por uso de Cheats.");
            }
            else
            {
              ZNet.instance.Kick(znetPeer.m_playerName);
              ZLog.LogError("Jogador" + znetPeer.m_playerName + znetPeer.m_uid + znetPeer.m_characterID +
                            " Kickado por uso de Cheats.");
            }
          }
        }

        VACPlugin.toKick.Clear();
      }
    }
  }
}