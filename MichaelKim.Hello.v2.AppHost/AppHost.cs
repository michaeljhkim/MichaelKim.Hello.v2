var builder = DistributedApplication.CreateBuilder(args);

// Connection String is taken from Azure's environment config
//var connectionString = builder.AddConnectionString("hellodb");

// Backend
var apiService = builder.AddProject<Projects.MichaelKim_Hello_v2_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    //.WithReference(connectionString)
    //.WaitFor(connectionString)
    ;

// Frontend
builder.AddProject<Projects.MichaelKim_Hello_v2_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
