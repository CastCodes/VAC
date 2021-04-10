using HarmonyLib;
using UnityEngine;
using System;
using UnityEngine.Rendering;

namespace VAC.AntiParams.GamePatches
{
    [HarmonyPatch(typeof (ZNet), "OnNewConnection")]
    public class AddPlayer_Patch
    {
        public static void Postfix(ZNet __instance, ZNetPeer peer)
        {
          if (VACPlugin.posMap.ContainsKey(peer))
              return;
          VACPlugin.posMap.Add(peer, Vector3.zero);
        }
    }
}