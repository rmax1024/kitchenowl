using BackendWebApi.Core;
using BackendWebApi.Properties;
using Dapper;

namespace BackendWebApi.Common;

public class CommonRepository() : BaseRepository(Settings.Current.DatabaseConnectionString), ICommonRepository
{
    public async Task<bool> IsOnboarding()
    {
        await using var connection = GetConnection();
        var userCount = await connection.ExecuteScalarAsync<int>("select count(1) from user");
        return userCount == 0;
    }
}