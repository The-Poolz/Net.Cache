namespace Net.Cache;

/// <summary>
/// Provides storage of values by key through an internal dictionary.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
public class InternalStorageProvider<TKey, TValue> : IStorageProvider<TKey, TValue>
    where TKey : IEquatable<TKey>
    where TValue : notnull
{
    protected readonly Lazy<Dictionary<TKey, TValue>> lazyCache;
    protected Dictionary<TKey, TValue> Cache => lazyCache.Value;

    public InternalStorageProvider()
    {
        lazyCache = new Lazy<Dictionary<TKey, TValue>>();
    }

    public void Store(TKey key, TValue value) => Cache.Add(key, value);

    public bool TryGetValue(TKey key, out TValue value) => Cache.TryGetValue(key, out value!);
}