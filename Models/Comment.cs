using Cassandra;
using Newtonsoft.Json;
using DataStax.AstraDB.DataApi.Tables;

namespace kv_be_csharp_dataapi_table.Models;

public class Comment
{
    [ColumnPrimaryKey(1)]
    [ColumnName("videoid")]
    public Guid videoid { get; set; } = Guid.Empty;

    [ColumnPrimaryKey(2)]
    [ColumnName("commentid")]
    public TimeUuid commentid { get; set; } = TimeUuid.NewId();
    public string comment { get; set; } = string.Empty;
    public Guid userid { get; set; } = Guid.Empty;
    
    [ColumnName("sentiment_score")]
    [JsonProperty("sentiment_score")]
    public float sentimentScore { get; set; } = 0.0F;
}