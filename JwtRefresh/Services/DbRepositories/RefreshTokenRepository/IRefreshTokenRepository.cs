using JwtRefresh.Models;
using JwtRefresh.Models.AuthModels;

namespace JwtRefresh.Services.RefreshTokenRepository
{
    public interface IRefreshTokenRepository
    {
        Task<UserToken?> GetByToken(string token);
        
        Task Add(UserToken userTokensDto);
     
        Task Delete(Guid Id);

        Task DeleteAll(Guid userId);
    }
}
