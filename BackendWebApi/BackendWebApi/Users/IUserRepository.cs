using BackendWebApi.Users.Model;

namespace BackendWebApi.Users;

public interface IUserRepository
{
    Task<User?> GetById(int id);
    Task<User?> GetByUsername(string username);
}