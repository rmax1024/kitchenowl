namespace BackendWebApi.Properties;

public class JwtSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
    public TimeSpan TokenValidity { get; set; }
    public TimeSpan RefreshTokenValidity { get; set; }
}