using System;
using System.Linq;
using System.Collections.Generic;

namespace Net.Cache
{
    /// <summary>
    /// Manages caching of values identified by keys. This class allows for storing and retrieving values efficiently and 
    /// provides mechanisms for adding new values to the cache dynamically if they are not already present.
    /// </summary>
    /// <typeparam name="TKey">The type of keys used for identifying values. Must be equatable and non-nullable.</typeparam>
    /// <typeparam name="TValue">The type of values to be cached. This type is non-nullable.</typeparam>
    public class CacheProvider<TKey, TValue>
        where TKey : IEquatable<TKey>
        where TValue : notnull
    {
        protected readonly List<IStorageProvider<TKey, TValue>> storageProviders;

        /// <inheritdoc cref="ICacheProvider{TKey, TValue}.GetOrAdd(TKey, Func{TValue})"/>
        public virtual TValue GetOrAdd(TKey key, Func<TValue> valueFactory)
        {
            return GetOrAddInternal(key, _ => valueFactory());
        }

        /// <inheritdoc cref="ICacheProvider{TKey, TValue}.GetOrAdd(TKey, Func{object[], TValue}, object[])"/>
        public virtual TValue GetOrAdd(TKey key, Func<object[], TValue> valueFactory, params object[] args)
        {
            return GetOrAddInternal(key, valueFactory, args);
        }

        /// <summary>
        /// Retrieves a value by key, or adds it to the cache using a parameter-less factory function if it's not already present.
        /// This method is useful when the creation of the value does not require any parameters.
        /// </summary>
        /// <param name="key">The key for retrieving or adding the value.</param>
        /// <param name="valueFactory">A function that creates a value when required. It takes no parameters.</param>
        /// <returns>The cached or newly added value associated with the specified <paramref name="key"/>.</returns>
        public virtual TValue GetOrAdd(TKey key, Func<TValue> valueFactory)
        {
            return GetOrAddInternal(key, _ => valueFactory());
        }

        /// <summary>
        /// Retrieves a value by key, or adds it to the cache using a factory function with parameters if it's not already present.
        /// This method allows for more complex value creation scenarios where parameters are needed.
        /// </summary>
        /// <param name="key">The key for retrieving or adding the value.</param>
        /// <param name="valueFactory">A function that creates a value when required. This function can take any number of parameters.</param>
        /// <param name="args">The arguments to pass to the <paramref name="valueFactory"/> function.</param>
        /// <returns>The cached or newly added value associated with the specified <paramref name="key"/>.</returns>
        public virtual TValue GetOrAdd(TKey key, Func<object[], TValue> valueFactory, params object[] args)
        {
            return GetOrAddInternal(key, valueFactory, args);
        }

        /// <summary>
        /// Internal method for handling the retrieval or addition of values in the cache.
        /// </summary>
        /// <param name="key">The key for the value.</param>
        /// <param name="valueFactory">The factory function for creating the value if it is not found in the cache.</param>
        /// <param name="args">Optional arguments for the factory function.</param>
        /// <returns>The value associated with the specified <paramref name="key"/>.</returns>
        protected virtual TValue GetOrAddInternal(TKey key, Func<object[], TValue> valueFactory, params object[] args)
        {
            foreach (var provider in storageProviders)
            {
                if (!provider.TryGetValue(key, out var storedValue))
                {
                    continue;
                }
                if (provider != storageProviders[0])
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
}