using System.Text;
using BackendWebApi.Common;
using BackendWebApi.Households;
using BackendWebApi.Properties;
using BackendWebApi.Users;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace BackendWebApi.Startup;

public static class ServicesExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services
            .AddSingleton<ICommonRepository, CommonRepository>()
            .AddSingleton<IUserRepository, UserRepository>()
            .AddSingleton<IHouseholdRepository, HouseholdRepository>();
        return services;
    }

    public static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(
                options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidAudience = Settings.Current.JwtSettings.Audience,
                        ValidIssuer = Settings.Current.JwtSettings.Issuer,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.Current.JwtSettings.Key))
                    };
                });
        services.AddAuthorization();
        return services;
    }

    public static IServiceCollection AddFastEndpointServices(this IServiceCollection services)
    {
        services
            .AddSingleton(typeof(IRequestBinder<>), typeof(CoreRequestBinder<>))
            .AddFastEndpoints().SwaggerDocument();
        return services;
    }
}