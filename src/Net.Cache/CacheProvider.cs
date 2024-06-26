using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Net.Cache
{
    /// <summary>
    /// Manages caching of values identified by keys. This class allows for storing and retrieving values efficiently and 
    /// provides mechanisms for adding new values to the cache dynamically if they are not already present.
    /// </summary>
    /// <typeparam name="TKey">The type of keys used for identifying values. Must be equatable and non-nullable.</typeparam>
    /// <typeparam name="TValue">The type of values to be cached. This type is non-nullable.</typeparam>
    public class CacheProvider<TKey, TValue> : ICacheProvider<TKey, TValue>
        where TKey : IEquatable<TKey>
        where TValue : notnull
    {
        protected readonly List<IStorageProvider<TKey, TValue>> storageProviders;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheProvider{TKey, TValue}"/> class using a collection of storage providers.
        /// Each storage provider can be a different mechanism for storing and retrieving values.
        /// </summary>
        /// <param name="storageProviders">An enumeration of storage providers used for value storage and retrieval.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="storageProviders"/> argument is <see langword="null"/>.</exception>
        public CacheProvider(IEnumerable<IStorageProvider<TKey, TValue>> storageProviders)
            : this(storageProviders.ToArray())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheProvider{TKey, TValue}"/> class using an array of storage providers.
        /// This constructor is typically used when you have a fixed number of storage providers.
        /// </summary>
        /// <param name="storageProviders">An array of storage providers used for value storage and retrieval.</param>
        public CacheProvider(params IStorageProvider<TKey, TValue>[] storageProviders)
        {
            this.storageProviders = storageProviders.ToList();
        }

        /// <inheritdoc cref="ICacheProvider{TKey, TValue}.GetOrAdd(TKey, Task{TValue})"/>
        public virtual TValue GetOrAdd(TKey key, Task<TValue> value)
        {
            return GetOrAddInternal(key, _ => value.GetAwaiter().GetResult());
        }

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
        
        /// <inheritdoc cref="ICacheProvider{TKey, TValue}.Get(TKey)"/>
        public TValue Get(TKey key)
        {
            foreach (var provider in storageProviders)
            {
                if (provider.TryGetValue(key, out var value))
                {
                    return value;
                }
            }
            throw new KeyNotFoundException($"The value associated with the key '{key}' was not found in any storage provider.");
        }

        /// <inheritdoc cref="ICacheProvider{TKey, TValue}.TryGet(TKey, out TValue)"/>
        public bool TryGet(TKey key, out TValue value)
        {
            value = default!;
            try
            {
                foreach (var provider in storageProviders)
                {
                    if (provider.TryGetValue(key, out value!))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        /// <inheritdoc cref="ICacheProvider{TKey, TValue}.Add(TKey, TValue)"/>
        public void Add(TKey key, TValue value) => storageProviders[0].Store(key, value);

        /// <inheritdoc cref="ICacheProvider{TKey, TValue}.Delete(TKey)"/>
        public void Delete(TKey key) => storageProviders.ForEach(provider => provider.Remove(key));

        /// <inheritdoc cref="ICacheProvider{TKey, TValue}.Update(TKey, TValue)"/>
        public void Update(TKey key, TValue value) => storageProviders.ForEach(provider => provider.Update(key, value));

        /// <inheritdoc cref="ICacheProvider{TKey, TValue}.ContainsKey(TKey)"/>
        public bool ContainsKey(TKey key) => storageProviders.Exists(provider => provider.ContainsKey(key));

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