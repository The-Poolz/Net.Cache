using Net.Cache.DynamoDb.ERC20.Models;

namespace Net.Cache.DynamoDb.ERC20;

public sealed class ERC20CacheProvider
{
    private readonly ERC20StorageProvider storageProvider;

    public ERC20CacheProvider()
        : this(new ERC20StorageProvider())
    { }

    public ERC20CacheProvider(ERC20StorageProvider storageProvider)
    {
        this.storageProvider = storageProvider;
    }

    public ERC20DynamoDbTable GetOrAdd(string key, GetCacheRequest request)
    {
        if (storageProvider.TryGetValue(key, request, out var storedValue))
        {
            return storedValue;
        }

        storedValue = new ERC20DynamoDbTable(request.ChainId, request.ERC20Service);
        storageProvider.Store(storedValue.HashKey, storedValue);
        return storedValue;
    }
}