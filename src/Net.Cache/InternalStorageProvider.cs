namespace Net.Cache;

/// <summary>
/// Implements the <see cref="IStorageProvider{TKey, TValue}"/> interface using an internal dictionary. 
/// This class provides a basic in-memory storage mechanism, allowing values to be stored and retrieved using keys.
/// </summary>
/// <typeparam name="TKey">The type of keys used in the storage, ensuring unique identification of values.</typeparam>
/// <typeparam name="TValue">The type of values to be stored, required to be non-nullable.</typeparam>
public class InternalStorageProvider<TKey, TValue> : IStorageProvider<TKey, TValue>
    where TKey : IEquatable<TKey>
    where TValue : notnull
{
    protected readonly Lazy<Dictionary<TKey, TValue>> lazyCache;
    protected Dictionary<TKey, TValue> Cache => lazyCache.Value;

    /// <summary>
    /// Initializes a new instance of the <see cref="InternalStorageProvider{TKey, TValue}"/> class.
    /// Sets up lazy initialization for the internal dictionary that stores the key-value pairs.
    /// </summary>
    public InternalStorageProvider()
    {
        lazyCache = new Lazy<Dictionary<TKey, TValue>>();
    }

    /// <inheritdoc cref="IStorageProvider{TKey, TValue}.Store(TKey, TValue)"/>
    public void Store(TKey key, TValue value) => Cache.Add(key, value);

    /// <inheritdoc cref="IStorageProvider{TKey, TValue}.TryGetValue(TKey, out TValue)"/>
    public bool TryGetValue(TKey key, out TValue value) => Cache.TryGetValue(key, out value!);
}