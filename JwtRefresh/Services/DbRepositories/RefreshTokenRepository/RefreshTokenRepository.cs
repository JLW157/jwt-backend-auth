using JwtRefresh.Models;
using JwtRefresh.Models.AuthModels;
using JwtRefresh.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace JwtRefresh.Services.RefreshTokenRepository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext context;

        public RefreshTokenRepository(AppDbContext appDbContext)
        {
            this.context = appDbContext;
        }

        public async Task Add(UserToken userTokens)
        {
            await context.UserTokens.AddAsync(userTokens);
        }

        public async Task Delete(Guid id)
        {
            var res = await context.UserTokens.Where(r => r.Id == id).ToListAsync();
            if (res.Count == 0)
            {
                throw new Exception("Invalid id, token not found!");
            }

            context.UserTokens.RemoveRange(res);
        }

        public async Task DeleteAll(Guid userId)
        {
            var res = await context.UserTokens.Where(r => r.UserId == userId).ToListAsync();

            if (res.Count == 0)
            {
                throw new Exception("Invalid userId, token not found!");
            }

            context.UserTokens.RemoveRange(res);
        }

        public async Task<UserToken?> GetByToken(string token)
        {
            var userToken = await context.UserTokens.FirstOrDefaultAsync(r => r.Token == token);

            if (userToken == null)
            {
                return null;
            }

            return userToken;
        }
    }
}
