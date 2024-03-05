using System.Text.Json.Serialization;

namespace BackendWebApi.Users;

public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string Username { get; set; }

    [JsonIgnore]
    public string Password { get; set; }
    public string? Photo { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool? Admin { get; set; }
    public string? Email { get; set; }
    public bool? EmailVerified { get; set; }
}