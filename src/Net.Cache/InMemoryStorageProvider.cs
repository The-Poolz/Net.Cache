using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Net.Cache
{
    /// <summary>
    /// Implements the <see cref="IStorageProvider{TKey, TValue}"/> interface using an internal dictionary. 
    /// This class provides a basic in-memory storage mechanism, allowing values to be stored and retrieved using keys.
    /// </summary>
    /// <typeparam name="TKey">The type of keys used in the storage, ensuring unique identification of values.</typeparam>
    /// <typeparam name="TValue">The type of values to be stored, required to be non-nullable.</typeparam>
    public class InMemoryStorageProvider<TKey, TValue> : IStorageProvider<TKey, TValue>
        where TKey : IEquatable<TKey>
        where TValue : notnull
    {
        protected readonly Lazy<IDictionary<TKey, TValue>> lazyCache;
        protected IDictionary<TKey, TValue> Cache => lazyCache.Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryStorageProvider{TKey, TValue}"/> class.
        /// </summary>
        public InMemoryStorageProvider()
        {
            lazyCache = new Lazy<IDictionary<TKey, TValue>>(() => new Dictionary<TKey, TValue>());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryStorageProvider{TKey, TValue}"/> class with initial values.
        /// </summary>
        /// <param name="initialValues">The dictionary containing the initial key-value pairs to be stored in memory.</param>
        public InMemoryStorageProvider(IDictionary<TKey, TValue> initialValues)
        {
            lazyCache = new Lazy<IDictionary<TKey, TValue>>(() => initialValues);
        }


        /// <inheritdoc cref="IStorageProvider{TKey, TValue}.Store(TKey, TValue)"/>
        public virtual void Store(TKey key, TValue value) => Cache.Add(key, value);

        /// <inheritdoc cref="IStorageProvider{TKey, TValue}.TryGetValue(TKey, out TValue)"/>
        public virtual bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => Cache.TryGetValue(key, out value);
    }
}