namespace UserService.BLL.Common.Providers
{
    public static class CacheKeysProvider
    {
        private const string REFRESH_PREFIX = "refresh:";
        private const string ACTIVATE_PREFIX = "activate:";

        public static string GetRefreshKey(Guid userId) 
            => REFRESH_PREFIX + userId.ToString();

        public static string GetActivateKey(Guid userId) 
            => ACTIVATE_PREFIX + userId.ToString();
    }
}
