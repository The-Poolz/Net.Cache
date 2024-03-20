using System;
using System.Diagnostics.CodeAnalysis;

namespace Net.Cache
{
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
        /// Stores the given value with the specified key. This method serves for both adding new key-value pairs.
        /// </summary>
        /// <param name="key">The key under which the value will be stored, acting as a unique identifier.</param>
        /// <param name="value">The value to be stored.</param>
        public void Store(TKey key, TValue value);

        /// <summary>
        /// Attempts to retrieve the value associated with a specified key. If the key is found, the method returns <see langword="true"/>,
        /// otherwise <see langword="false"/>. This method provides a fail-safe way to retrieve values without throwing exceptions for missing keys.
        /// </summary>
        /// <param name="key">The key associated with the value to retrieve.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found;
        /// otherwise, the default value for the type of the <paramref name="value"/> parameter. This parameter is passed uninitialized.</param>
        /// <returns><see langword="true"/> if the value was found; otherwise, <see langword="false"/>.</returns>
        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value);

        /// <summary>
        /// Removes the value associated with the specified key from the storage. If the key does not exist, the operation is ignored.
        /// This method allows for the removal of key-value pairs, ensuring that the storage does not retain entries that are no longer needed.
        /// </summary>
        /// <param name="key">The key of the value to be removed. This key acts as a unique identifier for the value in the storage.</param>
        public void Remove(TKey key);

        /// <summary>
        /// Updates the value associated with the specified key in the storage.
        /// If the key exists, its corresponding value is updated. If the key does not exist, no action is taken.
        /// </summary>
        /// <param name="key">The key under which the value is stored. This key acts as a unique identifier for the value in the storage.</param>
        /// <param name="value">The new value to be associated with the specified key in the storage, if the key exists.</param>
        public void Update(TKey key, TValue value);

        /// <summary>
        /// Checks whether the storage contains an entry with the specified key.
        /// </summary>
        /// <param name="key">The key to check for presence in the storage.</param>
        /// <returns><see langword="true"/> if the storage contains an entry with the specified key; otherwise, <see langword="false"/>.</returns>
        public bool ContainsKey(TKey key);
    }
}