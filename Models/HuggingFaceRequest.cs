namespace kv_be_csharp_dataapi_table.Models;

public class HuggingFaceRequest
{
    public string model { get; set; } = string.Empty;
    public string text { get; set; } = string.Empty;
}