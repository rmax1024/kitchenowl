using BackendWebApi.Common.Model;
using BackendWebApi.Core;
using BackendWebApi.Properties;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using OneOf.Types;

namespace BackendWebApi.Common.Endpoints;

public class OnboardingEndpoint(ICommonRepository commonRepository) : EndpointWithoutRequest<OnboardingResult>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("onboarding");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendAsync(new OnboardingResult
        {
            Onboarding = await commonRepository.IsOnboarding()
        }, cancellation: ct);
    }

    public override void OnBeforeHandle(EmptyRequest req)
    {
        CoreResponseHandlerHelper.AddHeaders(HttpContext);
    }
}