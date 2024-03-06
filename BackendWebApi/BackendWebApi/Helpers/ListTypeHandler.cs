using System.Data;
using System.Text.Json;
using Dapper;

namespace BackendWebApi.Helpers;

public class ListTypeHandler : SqlMapper.TypeHandler<List<string>?>
{
    public override void SetValue(IDbDataParameter parameter, List<string>? value)
    {
        parameter.Value = JsonSerializer.Serialize(value);
    }

    public override List<string>? Parse(object value)
    {
        return JsonSerializer.Deserialize<List<string>>(value.ToString()!);
    }
}