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

    /// <summary>
    /// Initializes a new instance of the <see cref="CacheProvider{TKey, TValue}"/> class.
    /// </summary>
    /// <param name="storageProvider">The storage provider used for retrieving and storing values.</param>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="storageProvider"/> parameter is null.</exception>
    public CacheProvider(IStorageProvider<TKey, TValue> storageProvider)
    {
        this.storageProvider = storageProvider ?? throw new ArgumentNullException(nameof(storageProvider));
        cache = new Dictionary<TKey, TValue>();
    }

    /// <summary>
    /// Gets a value from the cache by key. If the value is not present in the cache, it is created using the factory function and stored in the cache and storage.
    /// </summary>
    /// <param name="key">The key to look up the value.</param>
    /// <param name="valueFactory">The function that creates new values for caching.</param>
    /// <returns>The value associated with the key.</returns>
    public virtual TValue GetOrAdd(TKey key, Func<TValue> valueFactory)
    {
        return GetOrAddInternal(key, _ => valueFactory());
    }

    public virtual TValue GetOrAdd(TKey key, Func<object[], TValue> valueFactory, params object[] args)
    {
        return GetOrAddInternal(key, valueFactory, args);
    }

    private TValue GetOrAddInternal(TKey key, Func<object[], TValue> valueFactory, params object[] args)
    {
        if (!cache.TryGetValue(key, out var value))
        {
            value = storageProvider.TryGetValue(key, out var storedValue) ? storedValue : valueFactory(args);
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