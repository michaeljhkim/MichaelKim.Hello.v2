using Microsoft.Extensions.Caching.Memory;
// background service class -> what enables the fetcher to run in the background

/*
https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-9.0&tabs=net-cli#ihostedservice-interface
https://github.com/dotnet/samples/blob/main/core/workers/background-service/Worker.cs
*/

public class GitHubPinnedRepoService : BackgroundService {
    private readonly ILogger<GitHubPinnedRepoService> _logger;
    private readonly GitHubPinnedRepoFetcher _fetcher;
    // makes it so that the pinned repos are only fetched every 24 hours -> not technically nessecary, but really nice to have
    private readonly IMemoryCache _cache;

    // ILogger -> basic logging
    // GitHubPinnedRepoFetcher -> dependency injection
    // IMemoryCache -> explained in definition
    public GitHubPinnedRepoService(ILogger<GitHubPinnedRepoService> logger, GitHubPinnedRepoFetcher fetcher, IMemoryCache cache) {
        _logger = logger;
        _fetcher = fetcher;
        _cache = cache;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        while (!stoppingToken.IsCancellationRequested) {
            try {
                var repos = await _fetcher.FetchPinnedReposAsync("michaeljhkim");
                _cache.Set("PinnedRepos", repos, TimeSpan.FromHours(24));
                _logger.LogInformation("Fetched pinned repos at {Time}", DateTimeOffset.Now);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Failed to fetch pinned repos");
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
