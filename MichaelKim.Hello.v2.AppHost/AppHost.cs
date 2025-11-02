var builder = DistributedApplication.CreateBuilder(args);

/*
https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/external-parameters#connection-string-values
*/

// Connection String is taken from Azure's environment config
var connectionString = builder.AddConnectionString("hellodb");

// Backend
// for testing, use WithEnvironment, but withreference when uploading to azure
var apiService = builder.AddProject<Projects.MichaelKim_Hello_v2_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .WithReference(connectionString)
    .WaitFor(connectionString)
    ;

// Frontend
/*
builder.AddProject<Projects.MichaelKim_Hello_v2_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);
*/

builder.Build().Run();
