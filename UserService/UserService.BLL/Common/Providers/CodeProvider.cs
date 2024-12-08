namespace UserService.BLL.Common.Providers
{
    public static class CodeProvider
    {
        private static readonly Random _random = new Random();

        public static int GenerateSixDigitCode()
        {
            return _random.Next(100000, 999999);
        }
    }
}
