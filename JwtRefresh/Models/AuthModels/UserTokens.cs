namespace JwtRefresh.Models.AuthModels
{
    public class UserToken
    {
        public Guid Id { get; set; }

        public string Token { get; set; } = null!;

        public Guid UserId { get; set; }

        public User User { get; set; } = null!;
    }
}
