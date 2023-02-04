using JwtRefresh.Models;
using JwtRefresh.Models.AuthModels;
using JwtRefresh.Services.DbRepositories.RoleRespository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtRefresh.Services.TokenGenerators
{
    public class AccessTokenGenerator
    {
        private readonly JWTConfiguration configuration;
        private readonly TokenGenerator tokenGenerator;
        private readonly IRoleRepository roleRepository;

        public AccessTokenGenerator(IOptions<JWTConfiguration> options, TokenGenerator tokenGenerator, 
            IRoleRepository roleRepository)
        {
            this.configuration = options.Value;
            this.tokenGenerator = tokenGenerator;
            this.roleRepository = roleRepository;
        }

        public async Task<string> GenerateToken(User user)
        {
            var roleNames = await roleRepository.GetRolesOfUser(user);
            
            List<Claim> claims = new List<Claim>()
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
            };

            foreach (var role in roleNames)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return tokenGenerator.GenerateToken(configuration.AccessTokenSecret,
                configuration.Issuer, configuration.Audience,
                configuration.AccessTokenExpirationMinutes, claims);
        }
    }
}
