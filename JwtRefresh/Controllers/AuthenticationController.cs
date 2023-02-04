using JwtRefresh.Models;
using JwtRefresh.Models.AuthModels;
using JwtRefresh.Models.Data;
using JwtRefresh.Models.Requests;
using JwtRefresh.Models.Response;
using JwtRefresh.Services.Authenticators;
using JwtRefresh.Services.DbRepositories.RoleRespository;
using JwtRefresh.Services.PasswordHashers;
using JwtRefresh.Services.RefreshTokenRepository;
using JwtRefresh.Services.TokenGenerators;
using JwtRefresh.Services.TokenValidators;
using JwtRefresh.Services.UserRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace JwtRefresh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IUserRepository userRepository;
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IPasswordHasher passwordHasher;
        private readonly Authenticator authenticator;
        private readonly RefreshTokenValidator refreshTokenValidator;

        public AuthenticationController(IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IRoleRepository roleRepository,
            IPasswordHasher passwordHasher,
            RefreshTokenValidator refreshTokenValidator,
            Authenticator authenticator,
            AppDbContext context)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.refreshTokenRepository = refreshTokenRepository;
            this.passwordHasher = passwordHasher;
            this.refreshTokenValidator = refreshTokenValidator;
            this.authenticator = authenticator;
            this.context = context;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var user = await userRepository.GetByUsername(loginRequest.Username);

                if (user == null)
                {
                    return Unauthorized(new ErrorResponse("User with such username not found"));
                }

                if (!passwordHasher.VerifyPassword(loginRequest.Password, user.Password))
                {
                    return Unauthorized(new ErrorResponse("Invalid password"));
                }

                AuthenticatedUserResponse response = await authenticator.Authenticate(user);

                await context.SaveChangesAsync();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            try
            {
                bool isValidRefreshToken = await refreshTokenValidator.Validate(refreshRequest.RefreshToken);

                if (!isValidRefreshToken)
                {
                    return BadRequest(new ErrorResponse("Invalid refresh token!"));
                }

                var refreshTokenDto = await refreshTokenRepository.GetByToken(refreshRequest.RefreshToken);

                if (refreshTokenDto == null)
                {
                    return NotFound(new ErrorResponse("Invalid refresh token."));
                }

                await refreshTokenRepository.Delete(refreshTokenDto.Id);

                var user = await userRepository.GetById(refreshTokenDto.UserId);

                if (user == null)
                {
                    return NotFound(new ErrorResponse("User not found!"));
                }

                AuthenticatedUserResponse response = await authenticator.Authenticate(user);

                await context.SaveChangesAsync();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }

        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                User? existingUserByEmail = await userRepository.GetByEmail(registerRequest.Email);
                if (existingUserByEmail != null)
                {
                    return Conflict(new ErrorResponse("User alredy exists"));
                }

                User registrationUser = new User()
                {
                    Email = registerRequest.Email,
                    Age = registerRequest.Age,
                    Username = registerRequest.Username,
                    Password = passwordHasher.HashPassword(registerRequest.Password)
                };

                var role = await roleRepository.GetRoleByName("User"); 

                if (role == null)
                {
                    role = await roleRepository.AddRole("User");
                }

                if (!await roleRepository.AddUserToRole(registrationUser, role))
                {
                    return StatusCode(500, new ErrorResponse("Something went wrong when trying to add role to user"));
                }

                if(!await userRepository.Add(registrationUser))
                {
                    return StatusCode(500, new ErrorResponse("User not registered beacuse of server problems. Try again later."));
                }

                await context.SaveChangesAsync();

                return Ok("User registered successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var rawUserId = HttpContext.User.FindFirstValue("Id");

                if (!Guid.TryParse(rawUserId, out Guid userId))
                {
                    return Unauthorized();
                }

                await refreshTokenRepository.DeleteAll(userId);

                await context.SaveChangesAsync();

                return Ok("User logged out successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }
        }
    }
}
