namespace kv_be_csharp_dataapi_table.Models;

public class UserRegistrationRequest
{
    public string email { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
    public string firstname { get; set; } = string.Empty;
    public string lastname { get; set; } = string.Empty;
}