using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using BackendWebApi;
using BackendWebApi.Auth;
using BackendWebApi.Common;
using BackendWebApi.Helpers;
using BackendWebApi.Properties;
using BackendWebApi.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
Settings.Current = builder.Configuration.Get<Settings>()!;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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



var app = builder.Build();

Settings.Current.IsDevelopment = app.Environment.IsDevelopment();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/onboarding",
    async (ICommonRepository commonRepository) =>
        Results.Extensions.Object(new { onboarding = await commonRepository.IsOnboarding() }));//.RequireAuthorization();
app.MapGet("/api/health/{id}",
    () => Results.Extensions.Object(new
    {
        min_frontend_version = Settings.Current.MinFrontendVersion, msg = "OK", oidc_provider = Array.Empty<string>(),
        version = Settings.Current.Version
    }));
app.MapPost("/api/auth",
    async (HttpRequest request, IUserRepository userRepository) =>
    {
        var requestBody = await request.Body.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };
        var login = JsonSerializer.Deserialize<Login?>(requestBody, options);

        if (login == null || string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
        {
            return Results.BadRequest("Invalid user name or password");
        }

        var user = await userRepository.GetByUsername(login.Username);
        if (user == null)
        {
            return Results.BadRequest("Invalid user name or password");
        }

        var passwordHasher = new PasswordHasher<User>();
        var verificationResult = passwordHasher.VerifyHashedPassword(new User(), user.Password, login.Password);
        if (verificationResult == PasswordVerificationResult.Failed)
        {
            return Results.BadRequest("Invalid user name or password");
        }
        
        return Results.Extensions.Object(new
        {
            access_token = TokenGenerator.GetToken(user.Id, TokenType.Access),
            refresh_token = TokenGenerator.GetToken(user.Id, TokenType.Refresh),
            user
        });
    });

app.MapDelete("/api/auth", () => Results.Ok());

app.MapGet("/api/auth/refresh",
    (HttpRequest request, ICommonRepository commonRepository) =>
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadJwtToken(request.Headers.Authorization.First()?[7..]);
        var userId = int.Parse(token.Subject);
        return Results.Extensions.Object(new
        {
            access_token = TokenGenerator.GetToken(userId, TokenType.Access),
            refresh_token = TokenGenerator.GetToken(userId, TokenType.Refresh),
            user = new { id = 2, name = "test", username = "test", admin = 1 }
        });
    }).RequireAuthorization();

app.MapGet("/api/user",
        (HttpRequest request) =>
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(request.Headers.Authorization.First()?.Substring(7));
            return Results.Extensions.Object(new { id = 2, name = "test", username = "test", admin = true });
        })
    .RequireAuthorization();
app.MapGet("/api/household",
    (HttpRequest request) => Results.Extensions.Object(new[] { new { id = 2, name = "Home" } })).RequireAuthorization();



app.Run();