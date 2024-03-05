using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackendWebApi.Properties;
using Microsoft.IdentityModel.Tokens;

namespace BackendWebApi.Auth;

public class TokenGenerator
{
    public static string GetToken(int userId, TokenType type)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new("type", type.ToString().ToLower())
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.Current.JwtSettings.Key));

        var expirationTime = DateTime.UtcNow.Add(type switch
        {
            TokenType.Access => Settings.Current.JwtSettings.TokenValidity,
            TokenType.Refresh => Settings.Current.JwtSettings.RefreshTokenValidity,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        });
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expirationTime,
            Issuer = Settings.Current.JwtSettings.Issuer,
            Audience = Settings.Current.JwtSettings.Audience,
            SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);
        return jwt;
    }
}