using JwtRefresh.Models;
using JwtRefresh.Models.AuthModels;

namespace JwtRefresh.Services.UserRepositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmail(string email);

        Task<User?> GetByUsername(string username);
        
        Task<bool> Add(User user);

        Task<User?> GetById(Guid userId);
    }
}
