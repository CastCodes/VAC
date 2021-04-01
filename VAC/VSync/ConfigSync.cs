using System.Collections.Generic;
using System.IO;
using BepInEx;
using VConfig;

namespace VAC.VSync
{
    public class VACSyncConfig
    {

        static public bool isConnecting = false;
        public static void RPC_VACConfigSync(long sender, ZPackage configPkg)
        {
            if (ZNet.m_isServer) //Server
            {
                if (!Configuration.Current.Server.IsEnabled || !Configuration.Current.Server.serverSyncsConfig) return;

                ZPackage pkg = new ZPackage();

                string[] rawConfigData = File.ReadAllLines(Extra.ConfigIniPath);
                List<string> cleanConfigData = new List<string>();

                for (int i = 0; i < rawConfigData.Length; i++)
                {
                    if (rawConfigData[i].Trim().StartsWith(";") ||
                        rawConfigData[i].Trim().StartsWith("#")) continue; //Skip comments

                    if (rawConfigData[i].Trim().IsNullOrWhiteSpace()) continue; //Skip blank lines

                    //Add to clean data
                    cleanConfigData.Add(rawConfigData[i]);
                }

                //Add number of clean lines to package
                pkg.Write(cleanConfigData.Count);

                //Add each line to the package
                foreach (string line in cleanConfigData)
                {
                    pkg.Write(line);
                }

                ZRoutedRpc.instance.InvokeRoutedRPC(sender, "VACConfigSync", new object[]
                {
                    pkg
                });

                ZLog.Log("Synchronized Configuration #" + sender);
            }
            else //Client
            {
                if (configPkg != null && 
                    configPkg.Size() > 0 && 
                    sender == ZRoutedRpc.instance.GetServerPeerID()) //Validate the message is from the server and not another client.
                {
                    int numLines = configPkg.ReadInt();

                    if (numLines == 0)
                    {
                        ZLog.LogWarning("I received a zero-line configuration file from the server. Cannot load.");
                        return;
                    }

                    using (MemoryStream memStream = new MemoryStream())
                    {
                        using (StreamWriter tmpWriter = new StreamWriter(memStream))
                        {
                            for (int i = 0; i < numLines; i++)
                            {
                                string line = configPkg.ReadString();

                                tmpWriter.WriteLine(line);
                            }

                            tmpWriter.Flush(); //Flush to memStream
                            memStream.Position = 0; //Rewind stream

                            VAC.VACPlugin.harmony.UnpatchSelf();

                            // Sync HotKeys when connecting ?
                            if(Configuration.Current.Server.IsEnabled)
                            {
                                isConnecting = true;
                                Configuration.Current = Extra.LoadFromIni(memStream);
                                isConnecting = false;
                            }
                            else
                            {
                                Configuration.Current = Extra.LoadFromIni(memStream);
                            }
                                
                            VAC.VACPlugin.harmony.PatchAll();

                            ZLog.Log("Configuration successfully synchronized. (VAC)");
                        }
                    }
                }
            }
        }
    }
}