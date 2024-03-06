namespace BackendWebApi.Core;

public interface ITimeStamp
{
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}