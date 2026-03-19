using kv_be_csharp_dataapi_table.Models;

namespace kv_be_csharp_dataapi_table.Repositories;

public interface IUserCredentialsDAL
{
    UserCredentials SaveUserCreds(UserCredentials user);

    UserCredentials? FindByEmail(string email);

    void UpdateUserCreds(UserCredentials user);
}