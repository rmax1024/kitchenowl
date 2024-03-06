using System.Net;
using BackendWebApi.Auth.Model;
using BackendWebApi.Auth.Utils;
using BackendWebApi.Core;
using BackendWebApi.Users;
using BackendWebApi.Users.Model;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace BackendWebApi.Auth.Endpoints;

public class LoginEndpoint(IUserRepository userRepository) : Endpoint<Login, AuthResult>
{
    public override void Configure()
    {
        Post("auth");
        Description(x => x
            .Accepts<Login>("text/plain"));
        AllowAnonymous();
    }

    public override async Task HandleAsync(Login login, CancellationToken ct)
    {
        var user = await userRepository.GetByUsername(login.Username);
        if (user == null)
        {
            ThrowError("Invalid user name or password", (int)HttpStatusCode.BadRequest);
        }

        var passwordHasher = new PasswordHasher<User>();
        var verificationResult = passwordHasher.VerifyHashedPassword(new User(), user.Password, login.Password);
        if (verificationResult == PasswordVerificationResult.Failed)
        {
            ThrowError("Invalid user name or password", (int)HttpStatusCode.BadRequest);
        }

        await SendAsync(new AuthResult
        {
            AccessToken = TokenGenerator.GetToken(user.Id, TokenType.Access),
            RefreshToken = TokenGenerator.GetToken(user.Id, TokenType.Refresh),
            User = user
        }, cancellation: ct);
    }

    public override void OnBeforeHandle(Login req)
    {
        CoreResponseHandlerHelper.AddHeaders(HttpContext);
    }
}