using System.Data.Common;
using System.Runtime.InteropServices;
using Npgsql;

/*
https://learn.microsoft.com/en-us/dotnet/aspire/database/postgresql-integration?tabs=dotnet-cli
https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#register-groups-of-services-with-extension-methods
https://www.npgsql.org/doc/basic-usage.html

Dependency Injection class -> accesses the NpgsqlDataSource instance and runs sql commands
*/

public class DatabaseConnection {
    private readonly NpgsqlDataSource _dataSource;

    public DatabaseConnection(NpgsqlDataSource dataSource) {
        _dataSource = dataSource;
    }

    // Test method -> get's age from database
    public async Task<int?> GetAgeAsync() {
        await using var conn = await _dataSource.OpenConnectionAsync();

        await using var command = new NpgsqlCommand("SELECT age FROM hello_info LIMIT 1;", conn);
        // gets row(s)
        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

        // advance to first row -> reader starts at empty row
        if (await reader.ReadAsync()) {
            return reader.GetInt32(0);
        }

        // no rows found
        return null;
    }
}