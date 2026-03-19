using DataStax.AstraDB.DataApi;
using DataStax.AstraDB.DataApi.Core;

namespace kv_be_csharp_dataapi_table.Repositories;

public class CassandraConnection : ICassandraConnection
{
    private readonly string? _astraDbApplicationToken;
    private readonly string? _astraDbKeyspace;
    private readonly string? _astraApiEndpoint;

    public CassandraConnection()
    {
        _astraDbApplicationToken = System.Environment.GetEnvironmentVariable("ASTRA_DB_APPLICATION_TOKEN");
        _astraDbKeyspace = System.Environment.GetEnvironmentVariable("ASTRA_DB_KEYSPACE");
        _astraApiEndpoint = System.Environment.GetEnvironmentVariable("ASTRA_DB_API_ENDPOINT");
    }

    public Database GetDatabase()
    {
        // Instantiate the client
        var client = new DataApiClient();

        // Connect to a database
        var database = client.GetDatabase(
            _astraApiEndpoint,
            _astraDbApplicationToken,
            _astraDbKeyspace
            );

        return database;
    }
}