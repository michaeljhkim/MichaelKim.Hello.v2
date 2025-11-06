
using Microsoft.Extensions.Caching.Memory;
using Azure.Monitor.OpenTelemetry.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Enable CORS -> Secure later
var MyAllowSpecificOrigins = "AllowFrontend";
builder.Services.AddCors(options => {
    options.AddPolicy(MyAllowSpecificOrigins,
        builder => builder
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed((host) => true)
            .AllowAnyHeader());
});
builder.Services.AddControllers();  // might need if controllers are needed in the future

// Add OpenTelemetry and configure it to use Azure Monitor.
builder.Services.AddOpenTelemetry().UseAzureMonitor();

/*
- Connecting to Postgresql server database (Supabase) - configured by connection string in apphost
- Connection String is taken from Azure's environment config
*/
builder.AddNpgsqlDataSource("hellodb");
// database connection manager DI class
builder.Services.AddSingleton<DatabaseConnection>();

// Add services to the container.
builder.Services.AddProblemDetails();

// Register Pinned Repo Hosted Service
builder.Services.AddSingleton<GitHubPinnedRepoFetcher>();
builder.Services.AddHostedService<GitHubPinnedRepoService>();
builder.Services.AddMemoryCache();
builder.Services.AddControllers();  // might need if controllers are needed in the future

var app = builder.Build();
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.MapDefaultEndpoints();
app.MapControllers();

app.MapGet("/test", () => "Hello, World!");

app.MapGet("/pinned-repos", async (DatabaseConnection service) => {
        var data = await service.GetPinnedReposAsync();
        return Results.Ok(data);
    }
)
.WithName("GetPinnedRepos");

app.MapGet("/hello-info-data", async (DatabaseConnection service) => {
        var data = await service.GetHelloInfoDataAsync();
        return Results.Ok(data);
    }
);

app.MapGet("/hello-descriptions", async (DatabaseConnection service) => {
    var data = await service.GetHelloDescriptionDataAsync();
    return Results.Ok(data);
}
);

app.UseCors();

app.MapDefaultEndpoints();
app.Run();