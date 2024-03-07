using BackendWebApi.Helpers;
using Dapper;

namespace BackendWebApi.Startup;

public static class DapperConfiguration
{
    public static void Init()
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        SqlMapper.AddTypeHandler(new ListTypeHandler());
    }
}