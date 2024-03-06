using System.Text.Json.Serialization;
using BackendWebApi.Core;

namespace BackendWebApi.Users.Model;

public class User: ITimeStamp, IId, IName
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string Username { get; set; }

    [JsonIgnore]
    public string Password { get; set; }
    public string? Photo { get; set; }
    public bool? Admin { get; set; }
    public string? Email { get; set; }
    public bool? EmailVerified { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}