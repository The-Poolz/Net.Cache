namespace Net.Cache;

/// <summary>
/// Defines an interface for managing key-value based storage operations. It abstracts the underlying storage mechanism,
/// allowing for a variety of implementations while maintaining a consistent interface for storing and retrieving data.
/// </summary>
/// <typeparam name="TKey">The type used for keys in the storage, required to be equatable and non-nullable to ensure reliable value identification.</typeparam>
/// <typeparam name="TValue">The type of values stored, required to be non-nullable to ensure that each key is associated with a valid value.</typeparam>
public interface IStorageProvider<in TKey, TValue>
    where TKey : IEquatable<TKey>
    where TValue : notnull
{
    /// <summary>
    /// Stores the given value with the specified key. If a value with the same key already exists, it replaces the existing value.
    /// This method serves for both adding new key-value pairs.
    /// </summary>
    /// <param name="key">The key under which the value will be stored, acting as a unique identifier.</param>
    /// <param name="value">The value to be stored.</param>
    void Store(TKey key, TValue value);

    /// <summary>
    /// Attempts to retrieve the value associated with a specified key. If the key is found, the method returns <see langword="true"/>,
    /// otherwise <see langword="false"/>. This method provides a fail-safe way to retrieve values without throwing exceptions for missing keys.
    /// </summary>
    /// <param name="key">The key associated with the value to retrieve.</param>
    /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found;
    /// otherwise, the default value for the type of the <paramref name="value"/> parameter. This parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if the value was found; otherwise, <see langword="false"/>.</returns>
    bool TryGetValue(TKey key, out TValue value);
}