using BackendWebApi;
using BackendWebApi.Common;
using BackendWebApi.Properties;

var builder = WebApplication.CreateBuilder(args);
Settings.Current = builder.Configuration.Get<Settings>()!;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddSingleton<ICommonRepository, CommonRepository>();

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


app.Run();