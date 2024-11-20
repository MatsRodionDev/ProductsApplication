namespace UserService.BLL.Common.Providers
{
    public static class CacheKeysProvider
    {
        private static readonly string REFRESH_PREFIX = "refresh:";
        private static readonly string ACTIVATE_PREFIX = "activate:";

        public static string GetRefreshKey(Guid userId) 
            => REFRESH_PREFIX + userId.ToString();

        public static string GetActivateKey(Guid userId) 
            => ACTIVATE_PREFIX + userId.ToString();
    }
}
