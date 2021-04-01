using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using VConfig;

namespace VAC.AntiParams
{
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
            {
              VACPlugin.toKick.Add(peerByPlayerName);
              ZLog.LogWarning($"Player Banned {peerByPlayerName}, DebugFly or/and NoPlacementCost");
            }

            if (player.m_godMode && Configuration.Current.AntiParams.anti_god_mode && (peerByPlayerName != null &&
              !ZNet.instance.m_adminList.Contains(peerByPlayerName.m_rpc.GetSocket().GetHostName())))
            {
              VACPlugin.toKick.Add(peerByPlayerName);
              ZLog.LogWarning($"Player Banned {peerByPlayerName}, GodMode");
            }
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
              {
                VACPlugin.toKick.Add(peer);
                ZLog.LogWarning($"Player Banned {peer}, Fly Hack");
              }
              else
                VACPlugin.posMap[peer] = peer.m_refPos;
            }

            if (peer.IsReady() && !peer.m_characterID.IsNone() &&
                (ZNet.instance.m_zdoMan.GetZDO(peer.m_characterID).GetBool("DebugFly") &&
                 !ZNet.instance.m_adminList.Contains(peer.m_rpc.GetSocket().GetHostName())))
            {
              VACPlugin.toKick.Add(peer);
              ZLog.LogWarning($"Player Banned {peer}, Fly Hack");
            }
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
              ZLog.LogError("Player: " + znetPeer.m_playerName + znetPeer.m_uid + znetPeer.m_characterID +
                            " Bnned.");
            }
            else
            {
              ZNet.instance.Kick(znetPeer.m_playerName);
              ZLog.LogError("Player" + znetPeer.m_playerName + znetPeer.m_uid + znetPeer.m_characterID +
                            " Kicked.");
            }
          }
        }

        VACPlugin.toKick.Clear();
      }
    }
  }
}