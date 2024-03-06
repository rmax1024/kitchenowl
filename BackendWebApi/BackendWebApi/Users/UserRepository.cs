using BackendWebApi.Core;
using BackendWebApi.Properties;
using BackendWebApi.Users.Model;
using Dapper;

namespace BackendWebApi.Users;

public class UserRepository() : BaseRepository(Settings.Current.DatabaseConnectionString), IUserRepository
{
    public async Task<User?> GetById(int id)
    {
        await using var connection = GetConnection();
        var user = await connection.QueryFirstAsync<User?>("select * from user where id = @Id", new { Id = id});
        return user;
    }

    public async Task<User?> GetByUsername(string? username)
    {
        await using var connection = GetConnection();
        var user = await connection.QueryFirstAsync<User?>("select * from user where username = @Username", new { Username = username});
        return user;
    }
}