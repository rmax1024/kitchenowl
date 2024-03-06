using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BackendWebApi.Auth.Endpoints;

public class LogoutEndpoint : EndpointWithoutRequest<Ok>
{
    public override void Configure()
    {
        Delete("auth");
    }

    public override Task<Ok> ExecuteAsync(CancellationToken ct)
    {
        // TODO: add token removal
        return Task.FromResult(TypedResults.Ok());
    }
}