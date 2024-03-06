using BackendWebApi.Properties;

namespace BackendWebApi.Core;

public class CoreResponseHandlerHelper
{
    public static void AddHeaders(HttpContext context)
    {
        var (request, response) = (context.Request, context.Response);
        if (request.Headers.Referer.Count == 0) return;

        var referer = request.Headers.Referer[0]!.TrimEnd('/');

        var settings = Settings.Current;
        if (settings.IsDevelopment ||
            (!string.IsNullOrEmpty(settings.FrontendUrl) && referer.Equals(settings.FrontendUrl)))
        {
            response.Headers.AccessControlAllowOrigin = referer;
            response.Headers.AccessControlAllowCredentials = "true";
            response.Headers.AccessControlAllowHeaders =
                new[] { "Content-Type", "Cache-Control", "X-Requested-With" };
            response.Headers.AccessControlAllowMethods = "GET, POST, OPTIONS, PUT, DELETE";
        }
    }
}