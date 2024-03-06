using BackendWebApi.Common.Model;
using BackendWebApi.Core;
using BackendWebApi.Properties;
using FastEndpoints;

namespace BackendWebApi.Common.Endpoints;

public class HealthEndpoint() : EndpointWithoutRequest<HealthResult>
{
    public override void Configure()
    {
        Get("health/{InstanceId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        string? instanceId = Route<string>("InstanceId", false);
        await SendAsync(new HealthResult
        {
            MinFrontendVersion = Settings.Current.MinFrontendVersion,
            Msg = "OK",
            OidcProvider = Array.Empty<string>(),
            Version = Settings.Current.Version
        }, cancellation: ct);
    }

    public override void OnBeforeHandle(EmptyRequest req)
    {
        CoreResponseHandlerHelper.AddHeaders(HttpContext);
    }
}