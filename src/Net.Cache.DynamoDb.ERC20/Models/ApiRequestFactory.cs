﻿using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20.Api;
using Net.Cache.DynamoDb.ERC20.Models.Api;

namespace Net.Cache.DynamoDb.ERC20.Models
{
    /// <summary>
    /// A factory class for creating instances of API-related services and requests.
    /// </summary>
    /// <remarks>
    /// This factory class simplifies the creation of configurations, services, and requests
    /// for interacting with ERC20 tokens via API services.
    /// </remarks>
    public class ApiRequestFactory
    {
        /// <summary>
        /// Creates an instance of <see cref="ApiERC20ServiceConfig"/> using the provided API key, chain ID, contract address, and API URL.
        /// </summary>
        /// <param name="apiKey">The API key for accessing the ERC20 token data.</param>
        /// <param name="chainId">The blockchain chain ID where the ERC20 token resides.</param>
        /// <param name="contractAddress">The Ethereum address of the ERC20 token contract.</param>
        /// <param name="apiUrl">The API URL template for fetching the token data.</param>
        /// <returns>An instance of <see cref="ApiERC20ServiceConfig"/> configured with the provided parameters.</returns>
        public virtual ApiERC20ServiceConfig CreateApiServiceConfig(string apiKey, long chainId, EthereumAddress contractAddress, string apiUrl) => new ApiERC20ServiceConfig(apiKey, chainId, contractAddress, apiUrl);

        /// <summary>
        /// Creates an instance of <see cref="ApiERC20Service"/> using the provided configuration.
        /// </summary>
        /// <param name="config">The configuration object containing API key, chain ID, contract address, and API URL.</param>
        /// <returns>An instance of <see cref="ApiERC20Service"/> configured with the provided configuration.</returns>
        public virtual ApiERC20Service CreateApiService(ApiERC20ServiceConfig config) => new ApiERC20Service(config);

        /// <summary>
        /// Creates an instance of <see cref="GetCacheRequest"/> using the provided API service and chain ID.
        /// </summary>
        /// <param name="apiService">The API service for interacting with the ERC20 token.</param>
        /// <param name="chainId">The blockchain chain ID where the ERC20 token resides.</param>
        /// <returns>An instance of <see cref="GetCacheRequest"/> configured with the provided service and chain ID.</returns>
        public virtual GetCacheRequest CreateWithApiService(ApiERC20Service apiService, long chainId) => new GetCacheRequest(chainId, apiService);

        /// <summary>
        /// Creates an instance of <see cref="GetCacheRequest"/> by configuring and initializing the necessary API service.
        /// </summary>
        /// <param name="apiKey">The API key used to authenticate requests to the ERC20 token service.</param>
        /// <param name="chainId">The blockchain chain ID where the ERC20 token resides.</param>
        /// <param name="contractAddress">The Ethereum address of the ERC20 token contract.</param>
        /// <param name="apiUrl">The API URL template for fetching the token data.</param>
        /// <returns>An instance of <see cref="GetCacheRequest"/> configured with the appropriate service and chain ID.</returns>
        /// <remarks>
        /// This method simplifies the process of creating a <see cref="GetCacheRequest"/> by internally handling the
        /// creation of <see cref="ApiERC20ServiceConfig"/> and <see cref="ApiERC20Service"/> objects.
        /// </remarks>
        public virtual GetCacheRequest CreateCacheRequest(string apiKey, long chainId, EthereumAddress contractAddress, string apiUrl)
        {
            var config = new ApiERC20ServiceConfig(apiKey, chainId, contractAddress, apiUrl);
            var apiService = new ApiERC20Service(config);
            return new GetCacheRequest(chainId, apiService);
        }
    }
}