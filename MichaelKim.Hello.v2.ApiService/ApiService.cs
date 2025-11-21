using Microsoft.Extensions.Caching.Memory;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Enable CORS -> Secure later
var MyAllowSpecificOrigins = "AllowFrontend";
builder.Services.AddCors(options => {
    options.AddPolicy(
        MyAllowSpecificOrigins,
        builder => builder
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed((host) => true)
            .AllowAnyHeader()
        );
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

// Test endpoint: to see if endpoints work at all
app.MapGet("/test", () => "Hello, World!").RequireCors(MyAllowSpecificOrigins);

// checks if api-service can connect to the database
app.MapGet("/health-db", async (NpgsqlDataSource dataSource) => {
    try {
        var result = await dataSource.CreateCommand("SELECT 1").ExecuteScalarAsync();
        return result is 1 ? Results.Ok() : Results.Problem();
    }
    catch (Exception ex) {
        return Results.Problem(ex.Message);
    }
});

/*
- use dependency injection to streamline GET endpoints

- endpoint_name -> GET endpoint name for users
- sql_query -> sql_query for the command required to get data from database
- transform -> SqlQueryAsync returns list, so DI lambda required to either get data directly, or just keep it in list form
*/
void CreateGetEndpoint<T, TResult>(string endpoint_name, string sql_query, Func<List<T>, TResult> transform) where T : DataInterface, new() {

    app.MapGet(endpoint_name, async (DatabaseConnection service) => {
            var data = await service.SqlQueryAsync<T>(sql_query);
            var result = transform(data);
            return Results.Ok(result);
        }
    )
    .RequireCors(MyAllowSpecificOrigins);
}

CreateGetEndpoint<PinnedRepo, List<PinnedRepo>?>(
    "/pinned-repos", 
    "SELECT * FROM github_repos;",
    data => data.Any() ? data : null
);

CreateGetEndpoint<HelloInfoData, HelloInfoData?>(
    "/hello-info-data", 
    "SELECT * FROM hello_info LIMIT 1;",
    data => data.FirstOrDefault()
);

CreateGetEndpoint<HelloDescriptionData, HelloDescriptionData?>(
    "/hello-descriptions", 
    "SELECT * FROM hello_descriptions LIMIT 1;",
    data => data.FirstOrDefault()
);

app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();

app.MapDefaultEndpoints();
app.Run();