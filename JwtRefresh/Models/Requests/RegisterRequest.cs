using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace JwtRefresh.Models.Requests
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [Range(16, 120, ErrorMessage = "Age range is from 16 to 120")]
        public int Age { get; set; }
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        [MinLength(6, ErrorMessage = "Min length of password is 6 chars")]
        public string Password { get; set; } = null!;
    }
}
