using BackendWebApi.Users.Model;

namespace BackendWebApi.Auth.Model;

public class AuthResult
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public User User { get; set; }
}