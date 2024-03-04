namespace BackendWebApi.Properties
{
    public class Settings
    {
        public static Settings Current { get; set; } = new();
        public bool IsDevelopment { get; set; }
        public string FrontendUrl { get; set; }
        public int MinFrontendVersion { get; set; }
        public int Version { get; set; }
    }
}
