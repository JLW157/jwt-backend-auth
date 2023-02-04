using JwtRefresh.Models;
using JwtRefresh.Models.AuthModels;
using JwtRefresh.Models.Response;
using JwtRefresh.Services.RefreshTokenRepository;
using JwtRefresh.Services.TokenGenerators;

namespace JwtRefresh.Services.Authenticators
{
    public class Authenticator
    {
        private readonly AccessTokenGenerator accessTokenGenerator;
        private readonly RefreshTokenGenerator refreshTokenGenerator;
        private readonly IRefreshTokenRepository refreshTokenRepository;

        public Authenticator(AccessTokenGenerator accessTokenGenerator, 
            RefreshTokenGenerator refreshTokenGenerator, 
            IRefreshTokenRepository refreshTokenRepository)
        {
            this.accessTokenGenerator = accessTokenGenerator;
            this.refreshTokenGenerator = refreshTokenGenerator;
            this.refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<AuthenticatedUserResponse> Authenticate(User user)
        {
            string accessToken = await accessTokenGenerator.GenerateToken(user);
            string refreshToken = refreshTokenGenerator.GenerateToken();

            UserToken refreshTokenDto = new UserToken()
            {
                Token = refreshToken,
                User = user
            };

            await refreshTokenRepository.Add(refreshTokenDto);

            return new AuthenticatedUserResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
