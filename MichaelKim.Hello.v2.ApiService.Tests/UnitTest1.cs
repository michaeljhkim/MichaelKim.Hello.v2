namespace MichaelKim.Hello.v2.ApiService.Tests;

using System.Net;
using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

// study this more later
public class AppHostFixture : IAsyncLifetime {
    public DistributedApplication App { get; private set; }

    public async Task InitializeAsync() {
        var builder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.MichaelKim_Hello_v2_AppHost>();
        App = await builder.BuildAsync();
        await App.StartAsync();
    }

    public async Task DisposeAsync() {
        if (App != null)
            await App.DisposeAsync();
    }
}


public class IntegrationTests : IClassFixture<AppHostFixture> {
    // REMINDER: tests are to check if code is consistent 
    private readonly AppHostFixture _fixture;
    public IntegrationTests(AppHostFixture fixture) {
        _fixture = fixture;
    }

    // test if endpoints can be recieved
    [Fact]
    public async Task Can_Get_Endpoints() {
        var client = _fixture.App.CreateHttpClient("apiservice");
        var response = await client.GetAsync("/test");
        
        //Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        response.EnsureSuccessStatusCode();
    }

    // using an endpoint test -> check if apiservice can connect to database 
    [Fact]
    public async Task ApiService_Can_Connect_To_Database() {
        var client = _fixture.App.CreateHttpClient("apiservice");
        var response = await client.GetAsync("/health-db");
        
        //Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        response.EnsureSuccessStatusCode();
    }
}
