using VConfig.Sections;

namespace VConfig
{
    public class Configuration
    {
        public static Configuration Current { get; set; }
        public ServerConfiguration Server { get; set; }
        public AntiParamsConfiguration AntiParams { get; set; }
        public AntiModsConfiguration AntiMods { get; set; }
    }
}