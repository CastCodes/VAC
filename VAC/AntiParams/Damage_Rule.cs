using HarmonyLib;
using VConfig;

namespace VAC.AntiParams
{
    public class Damage_Rule
    {
        public static bool Execute(HitData hit)
        {
            if (Configuration.Current.AntiParams.IsEnabled)
            {
                if (!Configuration.Current.AntiParams.anti_debug_mode &&
                    !Configuration.Current.AntiParams.anti_damage_boost)
                    return true;

                Character senderChar = hit.GetAttacker();

                if (senderChar != null)
                    if (Configuration.Current.Server.debugmode)
                        ZLog.LogError("Send Char" + senderChar);

                if (senderChar != null && senderChar.IsPlayer())
                {
                    ZNetPeer peer = ZNet.instance.GetPeer(senderChar.GetInstanceID());
                    if (Configuration.Current.Server.debugmode)
                    {
                        ZLog.LogError("Player Detected, player:" + senderChar.GetInstanceID());
                        ZLog.LogError("Damage = " + hit.GetTotalDamage());
                    }

                    float damage = hit.GetTotalDamage();
                    if (peer != null &&
                        (!Configuration.Current.AntiParams.admins_bypass ||
                         !ZNet.instance.m_adminList.Contains(peer.m_rpc.GetSocket().GetHostName())) && damage > 1000f)
                    {
                        if (Configuration.Current.Server.debugmode)
                            ZLog.LogError("Player Detected with Damage Boost.");
                        VACPlugin.toKick.Add(peer);
                    }
                }
            }
            return true;
        }
    }
}