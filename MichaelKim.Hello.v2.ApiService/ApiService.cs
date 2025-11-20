using Microsoft.Extensions.Caching.Memory;

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

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapGet("/test", () => "Hello, World!").RequireCors(MyAllowSpecificOrigins);

// use dependency injection to streamline GET endpoints
void CreateGetEndpoint<T>(string endpoint_name, Func<DatabaseConnection, Task<T>> func) {

    app.MapGet(endpoint_name, async (DatabaseConnection service) => {
            var data = await func(service);
            return Results.Ok(data);
        }
    )
    .RequireCors(MyAllowSpecificOrigins);
}

CreateGetEndpoint("/pinned-repos", s => s.GetPinnedReposAsync());
CreateGetEndpoint("/hello-info-data", s => s.GetHelloInfoDataAsync());
CreateGetEndpoint("/hello-descriptions", s => s.GetHelloDescriptionDataAsync());

app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();

app.MapDefaultEndpoints();
app.Run();