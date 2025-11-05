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
}

public class HelloDescriptionData {
    
}

public class PinnedRepo {
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Link { get; set; } = "";
    // managed by database
    public Guid Id { get; set; } = Guid.Empty;
}


public class DatabaseConnection {
    private readonly NpgsqlDataSource _dataSource;

    public DatabaseConnection(NpgsqlDataSource dataSource) {
        _dataSource = dataSource;
    }

    // Test method -> get's age from database
    public async Task<HelloInfoData?> GetHelloInfoDataAsync() {
        await using var conn = await _dataSource.OpenConnectionAsync();

        await using var command = new NpgsqlCommand("SELECT * from hello_info LIMIT 1;", conn);
        // gets row(s)
        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

        // advance to first row -> reader starts at empty row
        if (await reader.ReadAsync()) {
            return new HelloInfoData {
                first_name = reader.GetString(1),
                last_name = reader.GetString(2),
                age = reader.GetInt32(3),
                email = reader.GetString(4),
                github = reader.GetString(5),
                linkedin = reader.GetString(6),
                birth_date = reader.GetString(7),
            };
        }

        // no rows found
        return null;
    }

    // Get Data Scraped Pinned repos from sql database
    public async Task<List<PinnedRepo>?> GetPinnedReposAsync() {
        await using var conn = await _dataSource.OpenConnectionAsync();

        await using var command = new NpgsqlCommand("SELECT * from github_repos;", conn);
        // gets row(s)
        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

        
        List<PinnedRepo> pinned_repos_list = [];

        // advance row -> reader starts at empty row
        while (await reader.ReadAsync()) {
            pinned_repos_list.Add(
                new PinnedRepo {
                    Name = reader.GetString(0),
                    Description = reader.GetString(1),
                    Link = reader.GetString(2),
                    Id = reader.GetGuid(3),
                }
            );
        }

        if (pinned_repos_list.Any()) {
            return pinned_repos_list;
        }

        // no rows found
        return null;
    }
}