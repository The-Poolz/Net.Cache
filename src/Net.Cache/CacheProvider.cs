namespace Net.Cache;

/// <summary>
/// Provides caching of values by key.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
public class CacheProvider<TKey, TValue> where TKey : notnull
{
    protected readonly IStorageProvider<TKey, TValue> storageProvider;
    protected readonly Dictionary<TKey, TValue> cache;
    protected readonly Func<TKey, TValue> valueFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="CacheProvider{TKey, TValue}"/> class.
    /// </summary>
    /// <param name="storageProvider">The storage provider used for retrieving and storing values.</param>
    /// <param name="valueFactory">The function that creates new values for caching.</param>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="storageProvider"/> or <paramref name="valueFactory"/> parameters are null.</exception>
    public CacheProvider(IStorageProvider<TKey, TValue> storageProvider, Func<TKey, TValue> valueFactory)
    {
        this.storageProvider = storageProvider ?? throw new ArgumentNullException(nameof(storageProvider));
        this.valueFactory = valueFactory ?? throw new ArgumentNullException(nameof(valueFactory));
        cache = new Dictionary<TKey, TValue>();
    }

    /// <summary>
    /// Gets a value from the cache by key. If the value is not present in the cache, it is created using the factory function and stored in the cache and storage.
    /// </summary>
    /// <param name="key">The key to look up the value.</param>
    /// <returns>The value associated with the key.</returns>
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

    /// <summary>
    /// Attempts to add a value to the cache with the specified key.
    /// </summary>
    /// <param name="key">The key under which to add the value.</param>
    /// <param name="value">The value to add.</param>
    /// <returns><see langword="true"/> if the value was successfully added to the cache; otherwise, <see langword="false"/>.</returns>
    public virtual bool TryAdd(TKey key, TValue value) => cache.TryAdd(key, value);
}