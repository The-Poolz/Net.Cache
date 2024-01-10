namespace Net.Cache;

/// <summary>
/// Defines an interface for a storage provider, enabling key-value based data storage and retrieval.
/// This interface abstracts the underlying storage mechanism, allowing for flexible implementations.
/// </summary>
/// <typeparam name="TKey">Specifies the type of keys used to identify stored values. TKey must be non-nullable.</typeparam>
/// <typeparam name="TValue">Specifies the type of values to be stored. TValue must be non-nullable.</typeparam>
public interface IStorageProvider<in TKey, TValue>
    where TKey : IEquatable<TKey>
    where TValue : notnull
{
    /// <summary>
    /// Stores a value associated with a specific key in the storage.
    /// If a value with the same key already exists, it will be overwritten.
    /// </summary>
    /// <param name="key">The key under which the value is stored. Must be unique in the context of the storage.</param>
    /// <param name="value">The value to be stored.</param>
    void Store(TKey key, TValue value);

    /// <summary>
    /// Attempts to retrieve a value associated with a specific key.
    /// </summary>
    /// <param name="key">The key whose value is to be retrieved.</param>
    /// <param name="value">When this method returns, contains the value associated with the specified key, if found;
    /// otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
    /// <returns>True if the value was found; otherwise, false.</returns>
    bool TryGetValue(TKey key, out TValue value);
}