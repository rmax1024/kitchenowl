using System.IdentityModel.Tokens.Jwt;
using System.Net;
using BackendWebApi.Auth.Model;
using BackendWebApi.Auth.Utils;
using BackendWebApi.Core;
using BackendWebApi.Helpers;
using BackendWebApi.Users;
using FastEndpoints;

namespace BackendWebApi.Auth.Endpoints;

public class TokenRefreshEndpoint(IUserRepository userRepository) : EndpointWithoutRequest<AuthResult>
{
    public override void Configure()
    {
        Get("auth/refresh");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userIdResult= this.HttpContext.Request.GetUserId();
        if (userIdResult.IsT1)
        {
            ThrowError("Invalid user", (int)HttpStatusCode.BadRequest);
        }

        int userId = userIdResult.AsT0;
        var user = await userRepository.GetById(userId);
        if (user == null)
        {
            ThrowError("Invalid user", (int)HttpStatusCode.BadRequest);
        }

        // TODO: add token update
        await SendAsync(new AuthResult
        {
            AccessToken = TokenGenerator.GetToken(userId, TokenType.Access),
            RefreshToken = TokenGenerator.GetToken(userId, TokenType.Refresh),
            User = user
        }, cancellation: ct);
    }

    public override void OnBeforeHandle(EmptyRequest req)
    {
        CoreResponseHandlerHelper.AddHeaders(HttpContext);
    }
}