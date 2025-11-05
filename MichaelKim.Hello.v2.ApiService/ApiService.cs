
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// ONLY FOR LOCAL TEST -> FOR CORS ORIGIN ISSUE
var MyAllowSpecificOrigins = "AllowFrontend";
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
        builder => builder
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed((host) => true)
            .AllowAnyHeader());
});
builder.Services.AddControllers();  // might need if controllers are needed in the future
// END

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

// Register Pinned Repo Hosted Service
builder.Services.AddSingleton<GitHubPinnedRepoFetcher>();
builder.Services.AddHostedService<GitHubPinnedRepoService>();
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

// COMMENT THIS OUT OR MODIFY TO BE USEFUL
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
).WithName("GetPinnedRepos").RequireCors(MyAllowSpecificOrigins);

// test endpoint -> get age from database
app.MapGet("/hello-info-data", async (DatabaseConnection service) => {
        var age = await service.GetHelloInfoDataAsync();
        return Results.Ok(age);
    }
);

// ONLY FOR LOCAL TEST -> FOR CORS ORIGIN ISSUE
app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();
// END

app.MapDefaultEndpoints();
app.Run();