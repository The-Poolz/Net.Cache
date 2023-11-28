namespace Net.Cache;

public class CacheProvider<TKey, TValue> where TKey : notnull
{
    protected readonly IStorageProvider<TKey, TValue> storageProvider;
    protected readonly Dictionary<TKey, TValue> cache;
    protected readonly Func<TKey, TValue> valueFactory;

    public CacheProvider(IStorageProvider<TKey, TValue> storageProvider, Func<TKey, TValue> valueFactory)
    {
        this.storageProvider = storageProvider ?? throw new ArgumentNullException(nameof(storageProvider));
        this.valueFactory = valueFactory ?? throw new ArgumentNullException(nameof(valueFactory));
        cache = new Dictionary<TKey, TValue>();
    }

    public virtual TValue GetOrAdd(TKey key)
    {
        if (!cache.TryGetValue(key, out var value))
        {
            value = storageProvider.TryGetValue(key, out var storedValue) ? storedValue : valueFactory(key);
            cache[key] = value;
            storageProvider.Store(key, value);
        }

        return value;
    }

    public virtual bool TryAdd(TKey key, TValue value) => cache.TryAdd(key, value);
}