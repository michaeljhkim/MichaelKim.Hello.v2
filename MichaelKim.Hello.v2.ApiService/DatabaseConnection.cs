using System.Data.Common;
using System.Runtime.InteropServices;
using Npgsql;

/*
https://learn.microsoft.com/en-us/dotnet/aspire/database/postgresql-integration?tabs=dotnet-cli
https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#register-groups-of-services-with-extension-methods
https://www.npgsql.org/doc/basic-usage.html

Dependency Injection class -> accesses the NpgsqlDataSource instance and runs sql commands
*/

public class HelloInfoData {
    public string first_name { get; set; } = "";
    public string last_name { get; set; } = "";
    public int age { get; set; } = 0;
    public string email { get; set; } = "";
    public string github { get; set; } = "";
    public string linkedin { get; set; } = "";
    public string birth_date { get; set; } = "";

    public HelloInfoData(NpgsqlDataReader reader) {
        first_name = reader.GetString(1);
        last_name = reader.GetString(2);
        age = reader.GetInt32(3);
        email = reader.GetString(4);
        github = reader.GetString(5);
        linkedin = reader.GetString(6);
        birth_date = reader.GetString(7);
    }
}

public class HelloDescriptionData {
    public string role { get; set; } = "";
    public string website_description { get; set; } = "";

    public string about_me { get; set; } = "";

    public HelloDescriptionData(NpgsqlDataReader reader) {
        role = reader.GetString(1);
        website_description = reader.GetString(2);
        about_me = reader.GetString(3);
    }
}

public class PinnedRepo {
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Link { get; set; } = "";
    // managed by database
    public Guid Id { get; set; } = Guid.Empty;

    public PinnedRepo(NpgsqlDataReader reader) {
        Name = reader.GetString(0);
        Description = reader.GetString(1);
        Link = reader.GetString(2);
        Id = reader.GetGuid(3);
    }
}


public class DatabaseConnection {
    private readonly NpgsqlDataSource _dataSource;

    public DatabaseConnection(NpgsqlDataSource dataSource) {
        _dataSource = dataSource;
    }

    // gets row(s)
    private async Task<List<T>> SqlQueryAsync<T>(string sql, Func<NpgsqlDataReader, T> map) {
        await using var conn = await _dataSource.OpenConnectionAsync();
        await using var cmd = new NpgsqlCommand(sql, conn);
        await using var reader = await cmd.ExecuteReaderAsync();

        var results = new List<T>();

        while (await reader.ReadAsync()) {
            results.Add(map(reader));
        }
        return results;
    }

    // Get data from hello_info table
    public async Task<HelloInfoData?> GetHelloInfoDataAsync() {
        var result = await SqlQueryAsync("SELECT * FROM hello_info LIMIT 1;", reader => new HelloInfoData(reader));
        return result.FirstOrDefault();
    }

    // Get website descriptions data from hello_descriptions
    public async Task<HelloDescriptionData?> GetHelloDescriptionDataAsync() {
        var result = await SqlQueryAsync("SELECT * FROM hello_descriptions LIMIT 1;", reader => new HelloDescriptionData(reader));
        return result.FirstOrDefault();
    }

    // Get Data-Scraped Pinned repos from sql database
    public async Task<List<PinnedRepo>?> GetPinnedReposAsync() {
        var result = await SqlQueryAsync("SELECT * FROM github_repos;", reader => new PinnedRepo(reader));
        return result.Any() ? result : null;
    }
}