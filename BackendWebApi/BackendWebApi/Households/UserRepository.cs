using BackendWebApi.Core;
using BackendWebApi.Households.Model;
using BackendWebApi.Properties;
using Dapper;

namespace BackendWebApi.Households;

public class HouseholdRepository() : BaseRepository(Settings.Current.DatabaseConnectionString), IHouseholdRepository
{
    public async Task<Household?> GetById(int id)
    {
        await using var connection = GetConnection();
        var household = await connection.QueryFirstAsync<Household?>("select * from household where id = @Id", new { Id = id});
        return household;
    }

    public async Task<IEnumerable<Household>> GetByUserId(int userId)
    {
        await using var connection = GetConnection();
        var household = await connection.QueryAsync<Household>(
            "select h.* from household h inner join household_member hm on h.id = hm.household_id where hm.user_id = @UserId",
            new { UserId = userId });
        return household;
    }
}