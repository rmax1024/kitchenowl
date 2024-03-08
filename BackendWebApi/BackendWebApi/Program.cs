using System.Text.Json;
using BackendWebApi.Properties;
using BackendWebApi.Startup;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);
Settings.Current = builder.Configuration.Get<Settings>()!;

//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuth()
                .AddRepositories()
                .AddFastEndpointServices();

Console.WriteLine($"Database path: {Settings.Current.DatabaseConnectionString}");


DapperConfiguration.Init();

var app = builder.Build();
Settings.Current.IsDevelopment = app.Environment.IsDevelopment();

app.UseFastEndpoints(
    c =>
    {
        c.Endpoints.RoutePrefix = "api";
        c.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    }).UseSwaggerGen();
app.Run();