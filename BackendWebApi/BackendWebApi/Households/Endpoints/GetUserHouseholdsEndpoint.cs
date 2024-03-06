using BackendWebApi.Helpers;
using BackendWebApi.Households.Model;
using FastEndpoints;
using System.Net;

namespace BackendWebApi.Households.Endpoints;

public class GetUserHouseholdsEndpoint(IHouseholdRepository householdRepository) : EndpointWithoutRequest<IEnumerable<Household>>
{
    public override void Configure()
    {
        Get("household");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userIdResult = HttpContext.Request.GetUserId();
        if (userIdResult.IsT1)
        {
            ThrowError("Invalid user", (int)HttpStatusCode.BadRequest);
        }

        int userId = userIdResult.AsT0;
        var households = await householdRepository.GetByUserId(userId);
        await SendAsync(households, cancellation: ct);
    }
}