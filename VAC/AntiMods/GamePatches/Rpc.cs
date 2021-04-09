using System.Collections.Generic;

namespace VAC.AntiMods.GamePatches
{
    internal class Rpc
    {
        public static Dictionary<long, bool> Clients = new Dictionary<long, bool>(10);

        public static void AcHandshake(ZRpc rpc, long sender, string hash)
        {
            ZLog.Log(ZNet.instance.IsServer() ? (object) "Server" : (object) ("Clientreceived AcHandshake from " + sender.ToString()));
            if (ZNet.instance.IsServer())
            {
                Rpc.Clients.Add(sender, VACPlugin.PluginsHash.Equals(hash));
                ZLog.Log((object) ("AC: Storing " + sender.ToString()));
                ZRoutedRpc.instance.InvokeRoutedRPC(sender, nameof (AcHandshake), (object) "");
            }
            else
            {
                ZLog.Log((object) "AC: Got server request, sending hash");
                ZRoutedRpc.instance.InvokeRoutedRPC(sender, nameof (AcHandshake), (object) "123451");
            }
        }

        public static void AcRoutedHandshake(long sender, ZPackage pkg)
        {
            ZLog.Log(ZNet.instance.IsServer()
                ? (object) "Server"
                : (object) ("Clientreceived AcRoutedHandshake from " + sender.ToString()));
            if (ZNet.instance.IsServer())
                ZLog.Log((object) ("AC: Storing " + sender.ToString()));
            else
                ZLog.Log((object) "AC: Got server request, sending hash");
        }
    }
}