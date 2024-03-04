using BackendWebApi;
using BackendWebApi.Properties;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Settings.Current = builder.Configuration.Get<Settings>()!;

var app = builder.Build();

Settings.Current.IsDevelopment = app.Environment.IsDevelopment();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/onboarding", () => Results.Extensions.Object(new {onboarding=false}));
app.MapGet("/api/health/{id}",
    () => Results.Extensions.Object(new
    {
        min_frontend_version = Settings.Current.MinFrontendVersion, msg = "OK", oidc_provider = Array.Empty<string>(),
        version = Settings.Current.Version
    }));

//.WithName("GetWeatherForecast")
//.WithOpenApi();

app.Run();