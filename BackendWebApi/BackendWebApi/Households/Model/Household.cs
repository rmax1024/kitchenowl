using BackendWebApi.Core;

namespace BackendWebApi.Households.Model;

public class Household : ITimeStamp, IId, IName
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Photo { get; set; }
    public string? Language { get; set; }
    public bool PlannerFeature { get; set; }
    public bool ExpensesFeature { get; set; }
    public List<string>? ViewOrdering { get; set; }
}