namespace Net.Cache.Tests.Mock;

internal class MockStorageProvider : InMemoryStorageProvider<string, string>
{
    public IDictionary<string, string> Storage => Cache;
    public static IDictionary<string, string> DefaultStorage => new Dictionary<string, string>
    {
        { "key 1", "value 1" },
        { "key 2", "value 2" },
        { "key 3", "value 3" }
    };

    public MockStorageProvider()
        : base(DefaultStorage)
    { }
}