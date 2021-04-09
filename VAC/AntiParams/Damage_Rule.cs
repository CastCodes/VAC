namespace VAC.AntiParams
{
    public class Damage_Rule
    {
        public static bool Execute(HitData hit)
        {
            if (VACPlugin.AntiParams_IsEnabled.Value)
            {
                if (!VACPlugin.anti_debug_mode.Value &&
                    !VACPlugin.anti_damage_boost.Value)
                    return true;

                Character senderChar = hit.GetAttacker();

                if (senderChar != null)
                    if (VACPlugin.debugmode.Value)
                        ZLog.LogError("Send Char" + senderChar);

                if (senderChar != null && senderChar.IsPlayer())
                {
                    ZNetPeer peer = ZNet.instance.GetPeer(senderChar.GetInstanceID());
                    if (VACPlugin.debugmode.Value)
                    {
                        ZLog.LogError("Player Detected, player:" + senderChar.GetInstanceID());
                        ZLog.LogError("Damage = " + hit.GetTotalDamage());
                    }

                    float damage = hit.GetTotalDamage();
                    if (peer != null &&
                        (!VACPlugin.admins_bypass.Value ||
                         !ZNet.instance.m_adminList.Contains(peer.m_rpc.GetSocket().GetHostName())) && damage > 1000f)
                    {
                        if (VACPlugin.debugmode.Value)
                            ZLog.LogError("Player Detected with Damage Boost.");
                        VACPlugin.toKick.Add(peer);
                    }
                }
            }
            return true;
        }
    }
}