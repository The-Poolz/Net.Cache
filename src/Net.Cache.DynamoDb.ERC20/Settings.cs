using EnvironmentManager;

namespace Net.Cache.DynamoDb.ERC20
{
    public static class Settings
    {
        private static readonly EnvManager EnvManager = new EnvManager();

        public static readonly string UrlCovalent = EnvManager.GetEnvironmentValue<string>("URL_COVALENT", true);
    }
}