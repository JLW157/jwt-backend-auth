using JwtRefresh.Models;
using JwtRefresh.Models.AuthModels;
using JwtRefresh.Models.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace JwtRefresh.Services.UserRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext context;

        public UserRepository(AppDbContext appDbContext)
        {
            this.context = appDbContext;
        }

        public async Task<bool> Add(User user)
        {
            try
            {
                await context.Users.AddAsync(user);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<User?> GetByEmail(string email)
        {
            var res = await context.Users.FirstOrDefaultAsync(u => u.Email == email);

            return res;
        }

        public async Task<User?> GetById(Guid userId)
        {
            var res = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return res;
        }

        public async Task<User?> GetByUsername(string username)
        {
            var res = await context.Users.FirstOrDefaultAsync(u => u.Username == username);

            return res;
        }
    }
}
