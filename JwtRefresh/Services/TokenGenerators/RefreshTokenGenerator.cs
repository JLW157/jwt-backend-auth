using JwtRefresh.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtRefresh.Services.TokenGenerators
{
    public class RefreshTokenGenerator
    {
        private readonly JWTConfiguration configuration;
        public RefreshTokenGenerator(IOptions<JWTConfiguration> options)
        {
            this.configuration = options.Value;
        }
        public string GenerateToken()
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.RefreshTokenSecret));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
            configuration.Issuer,
                configuration.Audience,
                null,
                DateTime.Now,
                DateTime.Now.AddMinutes(configuration.RefreshTokenExpirationMinutes),
                credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
