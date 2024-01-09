namespace Net.Cache;

/// <summary>
/// Represents a generic interface for a storage provider that can store and retrieve values.
/// </summary>
/// <typeparam name="TKey">The type of keys in the storage.</typeparam>
/// <typeparam name="TValue">The type of values in the storage.</typeparam>
public interface IStorageProvider<in TKey, TValue>
    where TKey : notnull
    where TValue : notnull
{
    /// <summary>
    /// Stores the specified value with the associated key.
    /// </summary>
    /// <param name="key">The key associated with the value to store.</param>
    /// <param name="value">The value to store.</param>
    void Store(TKey key, TValue value);

    /// <summary>
    /// Tries to get the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the value to get.</param>
    /// <param name="value">When this method returns, contains the <paramref name="value"/> associated with the specified <paramref name="key"/>, if the <paramref name="key"/> is found;
    /// otherwise, the <see langword="default"/> value for the type of the <paramref name="value"/> parameter.</param>
    /// <returns><see langword="true"/> if the storage contains an element with the specified key; otherwise, <see langword="false"/>.</returns>
    bool TryGetValue(TKey key, out TValue value);
}