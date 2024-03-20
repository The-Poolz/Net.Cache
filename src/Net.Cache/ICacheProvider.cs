using System;
using System.Collections.Generic;

namespace Net.Cache
{
    public interface ICacheProvider<in TKey, TValue>
        where TKey : IEquatable<TKey>
        where TValue : notnull
    {
        /// <summary>
        /// Retrieves a value by key, or adds it to the cache using a parameter-less factory function if it's not already present.
        /// This method is useful when the creation of the value does not require any parameters.
        /// </summary>
        /// <param name="key">The key for retrieving or adding the value.</param>
        /// <param name="valueFactory">A function that creates a value when required. It takes no parameters.</param>
        /// <returns>The cached or newly added value associated with the specified <paramref name="key"/>.</returns>
        public TValue GetOrAdd(TKey key, Func<TValue> valueFactory);

        /// <summary>
        /// Retrieves a value by key, or adds it to the cache using a factory function with parameters if it's not already present.
        /// This method allows for more complex value creation scenarios where parameters are needed.
        /// </summary>
        /// <param name="key">The key for retrieving or adding the value.</param>
        /// <param name="valueFactory">A function that creates a value when required. This function can take any number of parameters.</param>
        /// <param name="args">The arguments to pass to the <paramref name="valueFactory"/> function.</param>
        /// <returns>The cached or newly added value associated with the specified <paramref name="key"/>.</returns>
        public TValue GetOrAdd(TKey key, Func<object[], TValue> valueFactory, params object[] args);

        /// <summary>
        /// Retrieves the value associated with the specified key from the cache.
        /// </summary>
        /// <param name="key">The key of the value to retrieve.</param>
        /// <returns>The value associated with the specified key. If the key is not found in any of the storage providers, an exception is thrown.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the value associated with the specified key is not found in any of the storage providers.</exception>
        /// <remarks>
        /// This method iterates through the collection of storage providers, attempting to retrieve the value associated with the specified key.
        /// If the value is found in one of the storage providers, it is returned immediately.
        /// If the key is not found in any of the storage providers, a <see cref="KeyNotFoundException"/> is thrown.
        /// This method demonstrates an early exit strategy; it returns as soon as the value is found without iterating through the remaining storage providers.
        /// </remarks>
        public TValue Get(TKey key);

        /// <summary>
        /// Attempts to retrieve the value associated with the specified key from the cache without throwing an exception if the key is not found.
        /// </summary>
        /// <param name="key">The key of the value to retrieve.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value"/> parameter. This parameter is passed uninitialized.</param>
        /// <returns>true if the cache contains an element with the specified key; otherwise, false.</returns>
        /// <remarks>
        /// This method provides a way to retrieve values from the cache without the risk of throwing an exception if the key does not exist. It is particularly useful in scenarios where it is acceptable for the value to be missing, or where the presence of a value needs to be checked without causing an exception if it is not found.
        /// </remarks>
        public bool TryGet(TKey key, out TValue value);

        /// <summary>
        /// Adds a specified value to the cache with the specified key.
        /// </summary>
        /// <param name="key">The key under which the value is stored.</param>
        /// <param name="value">The value to be added to the cache.</param>
        public void Add(TKey key, TValue value);

        /// <summary>
        /// Removes the value associated with the specified key from the cache. If the key does not exist in the cache, the operation has no effect.
        /// This method facilitates the management of the cache by allowing the removal of obsolete or unnecessary entries, thereby maintaining the efficiency and relevance of the cache contents.
        /// </summary>
        /// <param name="key">The key of the value to be removed from the cache. This key acts as a unique identifier for the cache entry.</param>
        public void Delete(TKey key);

        /// <summary>
        /// Updates the value associated with the specified key in the cache.
        /// If the key exists, its corresponding value is updated. If the key does not exist, no action is taken.
        /// </summary>
        /// <param name="key">The key under which the value is stored. This key acts as a unique identifier for the cache entry.</param>
        /// <param name="value">The new value to be associated with the specified key in the cache, if the key exists.</param>
        public void Update(TKey key, TValue value);
    }
}