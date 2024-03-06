using System.Data.Common;
using BackendWebApi.Households.Model;

namespace BackendWebApi.Households;

public interface IHouseholdRepository
{
    Task<Household?> GetById(int id);
    Task<IEnumerable<Household>> GetByUserId(int userId);
    DbConnection GetConnection();
}