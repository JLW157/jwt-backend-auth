using JwtRefresh.Models.AuthModels;

namespace JwtRefresh.Services.DbRepositories.RoleRespository
{
    public interface IRoleRepository
    {
        Task<Role?> GetRoleByName(string roleName);

        Task<Role> AddRole(string roleName);

        Task<bool> AddUserToRole(User user, Role Role);

        Task<bool> RoleExists(string roleName);

        Task<List<string>> GetRolesOfUser(User user);
    }
}
