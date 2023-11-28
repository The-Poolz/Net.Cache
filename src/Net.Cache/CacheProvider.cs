namespace Net.Cache;

public class CacheProvider<TKey, TValue> where TKey : notnull
{
    protected readonly IStorageProvider<TKey, TValue> storageProvider;
    protected readonly Dictionary<TKey, TValue> cache;

    public CacheProvider(IStorageProvider<TKey, TValue> storageProvider)
    {
        this.storageProvider = storageProvider ?? throw new ArgumentNullException(nameof(storageProvider));
        cache = new Dictionary<TKey, TValue>();
    }

    public virtual TValue GetOrCache(TKey key, Func<TKey, TValue> valueFactory)
    {
        if (!cache.TryGetValue(key, out var value))
        {
            value = storageProvider.TryGetValue(key, out var storedValue) ? storedValue : valueFactory(key);
            cache[key] = value;
            storageProvider.Store(key, value);
        }

        return value;
    }

    public virtual void Cache(TKey key, TValue value)
    {
        if (!cache.TryAdd(key, value))
        {
            throw new ArgumentException($"An item with the same key has already been added. Key: {key}");
        }
    }
}