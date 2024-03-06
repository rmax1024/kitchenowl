using System.IdentityModel.Tokens.Jwt;
using OneOf;
using OneOf.Types;

namespace BackendWebApi.Helpers;

public static class RequestExtensions
{
    public static async Task<string> ReadAsStringAsync(this Stream requestBody, bool leaveOpen = false)
    {
        using StreamReader reader = new(requestBody, leaveOpen: leaveOpen);
        var bodyAsString = await reader.ReadToEndAsync();
        return bodyAsString;
    }

    public static OneOf<int, None> GetUserId(this HttpRequest request)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadJwtToken(request.Headers.Authorization.FirstOrDefault()?[7..]);
        return int.TryParse(token.Subject, out var userId)
            ? userId
            : new None();
    }
}