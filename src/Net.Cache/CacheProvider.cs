namespace Net.Cache;

/// <summary>
/// Provides caching of values by key.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
public class CacheProvider<TKey, TValue>
    where TKey : notnull
    where TValue : notnull
{
    protected readonly IStorageProvider<TKey, TValue>[] storageProviders;

    /// <summary>
    /// Initializes a new instance of the <see cref="CacheProvider{TKey, TValue}"/> class.
    /// </summary>
    /// <param name="storageProviders">The storage providers used for retrieving and storing values.</param>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="storageProviders"/> parameter is null.</exception>
    public CacheProvider(IEnumerable<IStorageProvider<TKey, TValue>> storageProviders)
        : this(storageProviders.ToArray())
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="CacheProvider{TKey, TValue}"/> class.
    /// </summary>
    /// <param name="storageProviders">The storage providers used for retrieving and storing values.</param>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="storageProviders"/> parameter is null.</exception>
    public CacheProvider(params IStorageProvider<TKey, TValue>[] storageProviders)
    {
        this.storageProviders = storageProviders ?? throw new ArgumentNullException(nameof(storageProviders));
    }

    /// <summary>
    /// Gets a value from the cache by key. If the value is not present in the cache, it is created using the parameter less factory function and stored in the cache and storage.
    /// </summary>
    /// <param name="key">The key to look up the value.</param>
    /// <param name="valueFactory">A parameter less function that creates new values for caching.</param>
    /// <returns>The value associated with the key.</returns>
    public virtual TValue GetOrAdd(TKey key, Func<TValue> valueFactory)
    {
        return GetOrAddInternal(key, _ => valueFactory());
    }

    /// <summary>
    /// Gets a value from the cache by key. If the value is not present in the cache, it is created using the factory function with parameters and stored in the cache and storage.
    /// </summary>
    /// <param name="key">The key to look up the value.</param>
    /// <param name="valueFactory">The function that creates new values for caching. This function can take any number of parameters.</param>
    /// <param name="args">The arguments to pass to the <paramref name="valueFactory"/> function.</param>
    /// <returns>The value associated with the key.</returns>
    public virtual TValue GetOrAdd(TKey key, Func<object[], TValue> valueFactory, params object[] args)
    {
        return GetOrAddInternal(key, valueFactory, args);
    }

    protected virtual TValue GetOrAddInternal(TKey key, Func<object[], TValue> valueFactory, params object[] args)
    {
        for (var i = 0; i < storageProviders.Length; i++)
        {
            if (!storageProviders[i].TryGetValue(key, out var storedValue))
            {
                continue;
            }
            if (i != 0)
            {
                storageProviders[0].Store(key, storedValue);
            }
            return storedValue;
        }

        var value = valueFactory(args);
        storageProviders[0].Store(key, value);
        return value;
    }
}