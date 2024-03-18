using System.Diagnostics.CodeAnalysis;

namespace Net.Cache.Tests.Mock;

internal class MockStorageProvider : IStorageProvider<string, string>
{
    public readonly IDictionary<string, string> storage;
    public static IDictionary<string, string> DefaultStorage => new Dictionary<string, string>
    {
        { "key 1", "value 1" },
        { "key 2", "value 2" },
        { "key 3", "value 3" }
    };

    public MockStorageProvider()
    {
        storage = DefaultStorage;
    }

    public MockStorageProvider(IDictionary<string, string> storage)
    {
        this.storage = storage;
    }

    public void Store(string key, string value)
    {
        storage.Add(key, value);
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out string value)
    {
        return storage.TryGetValue(key, out value);
    }
}