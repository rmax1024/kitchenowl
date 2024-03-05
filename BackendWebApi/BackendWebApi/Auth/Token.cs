namespace BackendWebApi.Auth;

public class Token
{
    public int Id { get; set; }
    public string Jti { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
    public DateTime LastUsedAt { get; set; }
    public int RefreshTokenId { get; set; }
    public int UserId { get; set; }
}