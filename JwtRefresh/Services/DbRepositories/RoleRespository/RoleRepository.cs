using JwtRefresh.Models.AuthModels;
using JwtRefresh.Models.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;

namespace JwtRefresh.Services.DbRepositories.RoleRespository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext context;

        public RoleRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Role> AddRole(string roleName)
        {
            try
            {
                Role role = new Role() { RoleName = roleName };
                await context.Roles.AddAsync(role);
                return role;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> AddUserToRole(User user, Role role)
        {
            UserRole userRole = new UserRole()
            {
                User = user,
                Role = role
            };

            try
            {
                await context.UserRoles.AddAsync(userRole);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Role?> GetRoleByName(string roleName)
        {
            var res = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);

            return res;
        }

        public async Task<List<string>> GetRolesOfUser(User user)
        {
            var roleNames = from role in context.Roles
                            join ur in context.UserRoles.Where(ur => ur.UserId == user.Id)
                            on role.Id equals ur.RoleId
                            select role.RoleName;

            return await roleNames.ToListAsync();
        }

        public async Task<bool> RoleExists(string roleName)
        {
            var res = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
            if (res == null)
            {
                return true;
            }

            return false;
        }
    }
}
