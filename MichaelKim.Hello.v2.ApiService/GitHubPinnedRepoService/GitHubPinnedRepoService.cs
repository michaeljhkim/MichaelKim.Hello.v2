using Microsoft.Extensions.Caching.Memory;
using Npgsql;
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
    private readonly NpgsqlDataSource _dataSource;

    // ILogger -> basic logging
    // GitHubPinnedRepoFetcher -> dependency injection
    // IMemoryCache -> explained in definition
    public GitHubPinnedRepoService(
        ILogger<GitHubPinnedRepoService> logger,
        GitHubPinnedRepoFetcher fetcher,
        IMemoryCache cache,
        NpgsqlDataSource dataSource
    ) {
        _logger = logger;
        _fetcher = fetcher;
        _cache = cache;
        _dataSource = dataSource;
    }

    // Data Scrape pinned repos from github profile -> save pinned repos to sql database
    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        while (!stoppingToken.IsCancellationRequested) {
            try {
                var repos = await _fetcher.FetchPinnedReposAsync("michaeljhkim");
                //_cache.Set("PinnedRepos", repos, TimeSpan.FromHours(24));
                _logger.LogInformation("Fetched pinned repos at {Time}", DateTimeOffset.Now);

                // Save Pinned Repos to sql database
                await SaveReposToDatabaseAsync(repos.OrderBy(r => r.Name), stoppingToken);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Failed to fetch pinned repos");
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }

    // Insert pinned repos into sql database
    private async Task SaveReposToDatabaseAsync(IEnumerable<PinnedRepo> repos, CancellationToken token) {
        await using var conn = await _dataSource.OpenConnectionAsync(token);

        // Clear the table -> old repo's removed 
        await using (var clearCmd = conn.CreateCommand()) {
            clearCmd.CommandText = "DELETE FROM github_repos;";
            await clearCmd.ExecuteNonQueryAsync(token);
        }

        // Insert fresh rows
        await using var cmd = conn.CreateCommand();
        foreach (var repo in repos) {
            cmd.CommandText = @"
                INSERT INTO github_repos (name, description, link)
                VALUES (@name, @desc, @link);
            ";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@name", repo.Name);
            cmd.Parameters.AddWithValue("@desc", repo.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@link", repo.Link);

            await cmd.ExecuteNonQueryAsync(token);
        }

        _logger.LogInformation("Saved {Count} repos to the database", repos.Count());
    }
}
