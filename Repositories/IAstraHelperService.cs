using kv_be_csharp_dataapi_table.Models;

namespace kv_be_csharp_dataapi_table.Repositories;

public interface IAstraHelperService
{
    Task<string?> PostDataAsyncAstra(string table, string query);

    Task<string?> FindByKeyValue(string table, string key, string value);
}