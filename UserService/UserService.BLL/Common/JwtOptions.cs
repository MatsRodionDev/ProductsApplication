namespace UserService.BLL.Common
{
    public class JwtOptions
    {
        public string SecretKey { get; set; } = string.Empty;
        public int AccesTokenExpiresMinutes { get; set; }
        public int RefreshTokenExpiresDays { get; set; }
    }
}
