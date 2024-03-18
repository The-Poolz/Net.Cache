using System;

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
    }
}