using JwtRefresh.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace JwtRefresh.Services.TokenValidators
{
    public class RefreshTokenValidator
    {
        private readonly JWTConfiguration configuration;

        public RefreshTokenValidator(IOptions<JWTConfiguration> configuration)
        {
            this.configuration = configuration.Value;
        }

        public async Task<bool> Validate(string refreshToken)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
         
            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration.Issuer,
                ValidAudience = configuration.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.RefreshTokenSecret)),
                ClockSkew = TimeSpan.Zero,
            };
            
            try
            {
                var res = await tokenHandler.ValidateTokenAsync(
                    refreshToken,
                    validationParameters);

                return res.IsValid;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
