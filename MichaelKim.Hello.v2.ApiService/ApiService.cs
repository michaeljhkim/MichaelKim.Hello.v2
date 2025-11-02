
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

/*
- Connecting to Postgresql server database (Supabase) - configured by connection string in apphost
- Connection String is taken from Azure's environment config
*/
builder.AddNpgsqlDataSource("hellodb");
// database connection manager DI class
builder.Services.AddSingleton<DatabaseConnection>();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

/*
Register Pinned Repo Hosted Service 
- GitHubPinnedRepoFetcher -> single global instance created and managed by builder.Services 
- GitHubPinnedRepoService -> backgroud service
*/
builder.Services.AddSingleton<GitHubPinnedRepoFetcher>();
builder.Services.AddHostedService<GitHubPinnedRepoService>();
builder.Services.AddMemoryCache();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment()) {
    app.MapOpenApi();
}

app.MapGet("/pinned-repos", (IMemoryCache cache) => {
        // PinnedRepos -> name in cache, set in GitHubPinnedRepoService
        if (cache.TryGetValue<List<PinnedRepo>>("PinnedRepos", out var repos)) {
            return Results.Ok(repos);
        }
        return Results.Ok(new List<PinnedRepo>()); // return empty if not fetched yet
    }
).WithName("GetPinnedRepos");

// test endpoint -> get age from database
app.MapGet("/age", async (DatabaseConnection service) => {
    var age = await service.GetAgeAsync();
    return Results.Ok(age);
});

/*
string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];
app.MapGet("/weatherforecast", () => {
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        )
    ).ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");
*/

app.MapDefaultEndpoints();
app.Run();

/*
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary) {
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
*/