using VAC.VTools;

namespace VAC.VTools
{
    public class VACAck
    {
        public static void RPC_VACAck(long sender)
        {
            RpcQueue.GotAck();
        }

        public static void SendAck(long target)
        {
            ZRoutedRpc.instance.InvokeRoutedRPC(target, "VACAck");
        }
    }
}