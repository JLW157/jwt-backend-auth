namespace JwtRefresh.Models.Response
{
    public class AuthenticatedUserResponse
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
