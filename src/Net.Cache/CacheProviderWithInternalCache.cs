namespace Net.Cache;

/// <summary>
/// Provides caching of values by key. Also has an internal dictionary as a cache.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
public class WithInternalCacheProvider<TKey, TValue> : CacheProvider<TKey, TValue> where TKey : notnull
{
    protected readonly Dictionary<TKey, TValue> cache;

    public WithInternalCacheProvider(IStorageProvider<TKey, TValue> storageProvider)
        : base(storageProvider)
    {
        cache = new Dictionary<TKey, TValue>();
    }

    /// <summary>
    /// Attempts to add a value to the cache with the specified key.
    /// </summary>
    /// <param name="key">The key under which to add the value.</param>
    /// <param name="value">The value to add.</param>
    /// <returns><see langword="true"/> if the value was successfully added to the cache; otherwise, <see langword="false"/>.</returns>
    public virtual bool TryAdd(TKey key, TValue value) => cache.TryAdd(key, value);

    protected override TValue GetOrAddInternal(TKey key, Func<object[], TValue> valueFactory, params object[] args)
    {
        if (!cache.TryGetValue(key, out var value))
        {
            value = base.GetOrAddInternal(key, valueFactory, args);
            cache[key] = value;
            return value;
        }
        return value;
    }
}