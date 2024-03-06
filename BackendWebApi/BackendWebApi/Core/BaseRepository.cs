using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace BackendWebApi.Core;

public class BaseRepository(string connectionString)
{
    public DbConnection GetConnection()
    {
        return new SqliteConnection(connectionString);
    } 
}