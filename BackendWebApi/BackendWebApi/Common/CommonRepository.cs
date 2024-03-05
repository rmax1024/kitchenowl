using BackendWebApi.Core;
using BackendWebApi.Properties;
using Dapper;

namespace BackendWebApi.Common;

public class CommonRepository : BaseRepository, ICommonRepository
{
    public CommonRepository() : base(Settings.Current.DatabaseConnectionString)
    {
    }

    public async Task<bool> IsOnboarding()
    {
        await using var connection = GetConnection();
        var userCount = await connection.ExecuteScalarAsync<int>("select count(1) from user");
        return userCount == 0;
    }
}