using System.ComponentModel.DataAnnotations;

namespace JwtRefresh.Models.Requests
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        [MinLength(6, ErrorMessage = "Min length of password is 6 chars")]
        public string Password { get; set; } = null!;
    }
}
