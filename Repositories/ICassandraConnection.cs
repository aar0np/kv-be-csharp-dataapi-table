using DataStax.AstraDB.DataApi.Core;

namespace kv_be_csharp_dataapi_table.Repositories;

public interface ICassandraConnection
{
    Database GetDatabase();
}