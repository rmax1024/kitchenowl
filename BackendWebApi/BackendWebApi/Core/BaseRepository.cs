using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace BackendWebApi.Core;

public class BaseRepository
{
    private readonly string _connectionString;

    public BaseRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    public DbConnection GetConnection()
    {
        return new SqliteConnection(_connectionString);
    } 
}