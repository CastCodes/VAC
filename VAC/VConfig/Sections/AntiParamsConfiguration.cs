namespace VConfig.Sections
{
    public class AntiParamsConfiguration : ServerSyncConfig<AntiParamsConfiguration>
    {
        public bool ban_on_trigger { get; set; } = false;
        public bool admins_bypass { get; set; } = false;
        public bool anti_fly { get; set; } = false;
        public bool anti_debug_mode { get; set; } = false;
        public bool anti_god_mode { get; set; } = false;
        public bool anti_damage_boost { get; set; } = false;
        public bool anti_health_boost { get; set; } = false;
    }
}