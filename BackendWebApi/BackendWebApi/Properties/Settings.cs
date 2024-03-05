using System.Text.Json.Serialization;

namespace BackendWebApi.Properties
{
    public class Settings
    {
        public static Settings Current { get; set; } = new();
        public bool IsDevelopment { get; set; }
        public string FrontendUrl { get; set; }
        public int MinFrontendVersion { get; set; }
        public int Version { get; set; }
        public string StoragePath { get; set; }
        public string DatabaseName { get; set; }

        public JwtSettings JwtSettings { get; set; }

        [JsonIgnore] public string DatabaseConnectionString => $"Data Source={Path.Combine(StoragePath, DatabaseName)}";
    }
}
