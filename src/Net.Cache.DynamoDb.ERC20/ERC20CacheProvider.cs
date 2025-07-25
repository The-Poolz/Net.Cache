﻿using Net.Cryptography.SHA256;
using Net.Cache.DynamoDb.ERC20.Models;
using System.Threading.Tasks;

namespace Net.Cache.DynamoDb.ERC20
{
    /// <summary>
    /// Provides functionality to manage the caching of ERC20 token information.
    /// </summary>
    /// <remarks>
    /// This provider encapsulates logic for retrieving and storing ERC20 token details
    /// within a DynamoDB cache. It supports operations to get existing cache entries or add new entries when required.
    /// </remarks>
    public class ERC20CacheProvider
    {
        private readonly ERC20StorageProvider storageProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ERC20CacheProvider"/> class with a default <see cref="ERC20StorageProvider"/> storage provider.
        /// </summary>
        public ERC20CacheProvider()
            : this(new ERC20StorageProvider())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ERC20CacheProvider"/> class with a specified <see cref="ERC20StorageProvider"/> storage provider.
        /// </summary>
        /// <param name="storageProvider">The storage provider to be used for caching.</param>
        public ERC20CacheProvider(ERC20StorageProvider storageProvider)
        {
            this.storageProvider = storageProvider;
        }

        /// <summary>
        /// Retrieves an ERC20 token information from the cache or adds it to the cache if it does not exist.
        /// </summary>
        /// <param name="request">The request containing details required to retrieve or add the ERC20 token information.</param>
        /// <returns>The ERC20 token information from the cache.</returns>
        /// <remarks>
        /// This method checks the cache for an existing entry for the specified ERC20 token.
        /// If the entry exists, it is returned. Otherwise, a new entry is created using the provided
        /// details in the request and added to the cache.
        /// </remarks>
        public virtual ERC20DynamoDbTable GetOrAdd(GetCacheRequest request)
        {
            if (storageProvider.TryGetValue($"{request.ChainId}-{request.ERC20Service.ContractAddress}".ToSha256(), request, out var storedValue))
            {
                return storedValue;
            }

            storedValue = new ERC20DynamoDbTable(request.ChainId, request.ERC20Service);
            storageProvider.Store(storedValue.HashKey, storedValue);
            return storedValue;
        }

        public virtual async Task<ERC20DynamoDbTable> GetOrAddAsync(GetCacheRequest request)
        {
            var (isExist, storedValue) = await storageProvider.TryGetValueAsync($"{request.ChainId}-{request.ERC20Service.ContractAddress}".ToSha256(), request);
            if (isExist && storedValue != null)
            {
                return storedValue;
            }

            storedValue = new ERC20DynamoDbTable(request.ChainId, request.ERC20Service);
            await storageProvider.StoreAsync(storedValue.HashKey, storedValue);
            return storedValue;
        }
    }
}