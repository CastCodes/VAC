using BepInEx;

namespace VAC.AntiMods.GamePatches
{
    public static class ZNet_RPC_PeerInfoPatch
    {
        private static bool Prefix(ref ZNet __instance, ZRpc rpc, ZPackage pkg)
        {
            if (__instance.IsServer())
            {
                string self = "";
                if (pkg.Size() > 32)
                {
                    pkg.SetPos(pkg.Size() - 32 - 1);
                    if (pkg.ReadByte() == (byte) 32)
                    {
                        pkg.SetPos(pkg.GetPos() - 1);
                        self = pkg.ReadString();
                    }
                }

                ZLog.Log((object) ("[AntiMods]: Got client hash: " + self + "\nmine: " + VACPlugin.PluginsHash));
                ZLog.LogWarning("Plugins Hash is Equals: " + !self.Equals(VACPlugin.PluginsHash) + " ForceMods: " + VACPlugin.forcesamemods.Value);
                ZLog.LogWarning("Is in Admin List: " + !ZNet.instance.m_adminList.Contains(rpc.GetSocket().GetHostName()) + "Admin Bypass: " + VACPlugin.adminbypass.Value);
                pkg.SetPos(0);
                if (VACPlugin.adminbypass.Value && ZNet.instance.m_adminList.Contains(rpc.GetSocket().GetHostName()))
                    return true;
                if (!self.Equals(VACPlugin.PluginsHash) && VACPlugin.forcesamemods.Value)
                {
                    int num = self.IsNullOrWhiteSpace() ? 3 : 99;
                    ZLog.Log((object) ("[AntiMods]: Kicking Client: " + rpc.GetSocket().GetEndPointString() + " (incompatible mods)"));
                    rpc.Invoke("Error", (object) num);
                    return false;
                }

                ZLog.Log((object) ("[AntiMods]: Accepting Client: " + rpc.GetSocket().GetEndPointString()));
            }
            return true;
        }

        private static void Postfix()
        {
            if (ZNet.instance.IsServer())
                return;
            ZLog.Log((object) string.Format("[{0}]: Send AcRoutedHandshake to {1} from client",
                (object) "AntiCheat", (object) HookedZRoutedRpc.GetServerPeerID(ZRoutedRpc.instance)));
            ZRoutedRpc.instance.InvokeRoutedRPC(HookedZRoutedRpc.GetServerPeerID(ZRoutedRpc.instance),
                "AcRoutedHandshake", (object) new ZPackage());
        }
    }
}