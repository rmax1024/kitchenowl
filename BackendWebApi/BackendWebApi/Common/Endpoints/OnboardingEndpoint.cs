using BackendWebApi.Common.Model;
using FastEndpoints;

namespace BackendWebApi.Common.Endpoints;

public class OnboardingEndpoint(ICommonRepository commonRepository) : EndpointWithoutRequest<OnboardingResult>
{
    public override void Configure()
    {
        Get("onboarding");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendAsync(new OnboardingResult
        {
            Onboarding = await commonRepository.IsOnboarding()
        }, cancellation: ct);
    }
}