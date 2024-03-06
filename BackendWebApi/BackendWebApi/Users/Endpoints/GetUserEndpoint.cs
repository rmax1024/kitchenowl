using BackendWebApi.Auth.Model;
using BackendWebApi.Auth.Utils;
using BackendWebApi.Helpers;
using BackendWebApi.Users.Model;
using FastEndpoints;
using System.Net;

namespace BackendWebApi.Users.Endpoints;

public class GetUserEndpoint(IUserRepository userRepository) : EndpointWithoutRequest<User>
{
    public override void Configure()
    {
        Get("user");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userIdResult = this.HttpContext.Request.GetUserId();
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

        await SendAsync(user, cancellation: ct);
    }
}