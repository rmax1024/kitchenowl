using System.Text;
using System.Text.Json;
using BackendWebApi.Common;
using BackendWebApi.Core;
using BackendWebApi.Helpers;
using BackendWebApi.Households;
using BackendWebApi.Properties;
using BackendWebApi.Users;
using Dapper;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
Settings.Current = builder.Configuration.Get<Settings>()!;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(options =>
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.Current.JwtSettings.Key))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddSingleton<ICommonRepository, CommonRepository>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IHouseholdRepository, HouseholdRepository>();
builder.Services.AddSingleton(typeof(IRequestBinder<>), typeof(CoreRequestBinder<>));
builder.Services.AddFastEndpoints().SwaggerDocument();

DefaultTypeMap.MatchNamesWithUnderscores = true;
SqlMapper.AddTypeHandler(new ListTypeHandler());

var app = builder.Build();

Settings.Current.IsDevelopment = app.Environment.IsDevelopment();

app.UseFastEndpoints(
    c =>
    {
        c.Endpoints.RoutePrefix = "api";
        c.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    }).UseSwaggerGen();
app.Run();