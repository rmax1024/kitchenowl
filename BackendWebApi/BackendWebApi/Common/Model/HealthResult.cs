namespace BackendWebApi.Common.Model;

public class HealthResult
{
    public int MinFrontendVersion { get; set; }
    public string Msg { get; set; }
    public string[] OidcProvider { get; set; }
    public int Version { get; set; }
}