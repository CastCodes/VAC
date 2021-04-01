namespace VConfig.Sections
{
    public class ServerConfiguration : BaseConfig<ServerConfiguration>
    {
        //public bool enforceMod { get; internal set; } = true;
        public bool serverSyncsConfig { get; internal set; } = false;
        public bool debugmode { get; set; } = false;
    }
}