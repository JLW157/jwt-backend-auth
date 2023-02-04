using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace JwtRefresh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("admins-route")]
        public string OnlyAdmin()
        {

            var userId = HttpContext.User.FindFirstValue("Id");
            var email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var userRoles = HttpContext.User.FindAll(ClaimTypes.Role);

            StringBuilder roles = new StringBuilder();

            foreach (var item in userRoles)
            {
                roles.Append(item);
                roles.Append(" ");
            }

            return $"This is admin route!\nId: {userId} | E-mail: {email} | {roles}";
        }

        [HttpGet]
        [Route("user-admin-route")]
        [Authorize(Roles = "Admin, User")]
        public string UserAdminRoute()
        {
            var userId = HttpContext.User.FindFirstValue("Id");
            var email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var userRoles = HttpContext.User.FindAll(ClaimTypes.Role);

            StringBuilder roles = new StringBuilder();

            foreach (var item in userRoles)
            {
                roles.Append(item);
                roles.Append(" ");
            }

            return $"Id: {userId} | E-mail: {email} | {roles}";
        }

        [HttpGet]
        [Route("everyone")]
        [AllowAnonymous]
        public string Everyone()
        {
            return $"This is anonymous route";
        }
    }
}
