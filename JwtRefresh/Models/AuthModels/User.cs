namespace JwtRefresh.Models.AuthModels
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public int Age { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
