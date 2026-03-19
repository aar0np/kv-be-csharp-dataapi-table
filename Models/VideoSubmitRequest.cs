using Newtonsoft.Json;

namespace kv_be_csharp_dataapi_table.Models;

public class VideoSubmitRequest
{
    public string youtubeUrl { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
    public HashSet<string> tags { get; set; } = new();
    
    [JsonProperty("user_id")]
    public Guid userId { get; set; } = Guid.NewGuid();
}