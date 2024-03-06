using System.Text.Json;
using FastEndpoints;

namespace BackendWebApi.Core;

public class CoreRequestBinder<TRequest> : RequestBinder<TRequest> where TRequest : notnull
{
    public override async ValueTask<TRequest> BindAsync(BinderContext ctx, CancellationToken ct)
    {
        if (!(ctx.HttpContext.Request.ContentType?.Contains("text/plain") ?? false))
        {
            return await base.BindAsync(ctx, ct);
        }
        
        var req = await JsonSerializer.DeserializeAsync<TRequest>(
            ctx.HttpContext.Request.Body, ctx.SerializerOptions, ct);

        return req!;
    }
}