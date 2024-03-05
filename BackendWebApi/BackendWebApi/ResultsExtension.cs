﻿using System.Text.Json;
using System.Net.Mime;
using BackendWebApi.Properties;

namespace BackendWebApi;
static class ResultsExtensions
{ 
    public static IResult Object(this IResultExtensions resultExtensions, object input)
    {
        ArgumentNullException.ThrowIfNull(resultExtensions);
        return new ObjectResult(input);
    }
}

class ObjectResult(object input) : IResult
{
    public Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        var result = JsonSerializer.Serialize(input, options);
        var request = httpContext.Request;
        
        if (request.Headers.Referer.Count == 0)
        {
            return httpContext.Response.WriteAsync(result);
        }

        var referer = request.Headers.Referer[0]!.TrimEnd('/');

        var settings = Settings.Current;
        if (!settings.IsDevelopment &&
            (string.IsNullOrEmpty(settings.FrontendUrl) || !referer.Equals(settings.FrontendUrl)))
        {
            return httpContext.Response.WriteAsync(result);
        }

        httpContext.Response.Headers.AccessControlAllowOrigin = referer;
        httpContext.Response.Headers.AccessControlAllowCredentials = "true";
        httpContext.Response.Headers.AccessControlAllowHeaders = new [] {"Content-Type", "Cache-Control", "X-Requested-With" };
        httpContext.Response.Headers.AccessControlAllowMethods = "GET, POST, OPTIONS, PUT, DELETE";

        return httpContext.Response.WriteAsync(result);
    }
}