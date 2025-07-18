﻿using System;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20.RPC;
using System.Threading.Tasks;

namespace Net.Cache.DynamoDb.ERC20.Models
{
    /// <summary>
    /// Represents a request to retrieve or update ERC20 token information in the cache.
    /// </summary>
    /// <remarks>
    /// This class encapsulates the details necessary for fetching or updating the cached data of an ERC20 token,
    /// including the block-chain chain ID, the <see cref="IERC20Service"/>, and a flag indicating whether to update the total supply.
    /// </remarks>
    public class GetCacheRequest
    {
        /// <summary>
        /// Gets the block-chain chain ID for the request.
        /// </summary>
        public long ChainId { get; }

        /// <summary>
        /// Gets the <see cref="IERC20Service"/> used to interact with the ERC20 token contract.
        /// </summary>
        public IERC20Service ERC20Service { get; }

        /// <summary>
        /// Gets a value indicating whether the total supply of the token should be updated in the cache.
        /// </summary>
        public bool UpdateTotalSupply { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCacheRequest"/> class with specified chain ID and contract address.
        /// </summary>
        /// <param name="chainId">The block-chain chain ID.</param>
        /// <param name="contractAddress">The ERC20 token contract address.</param>
        /// <param name="rpcUrl">The URL of the RPC endpoint to interact with the block-chain.</param>
        /// <param name="updateTotalSupply">Optional. Indicates whether to update the total supply of the token in the cache. Defaults to <see langword="true"/>.</param>
        /// <remarks>
        /// This constructor creates an instance of the <see cref="ERC20Service"/> class using the provided RPC URL and contract address.
        /// </remarks>
        public GetCacheRequest(long chainId, EthereumAddress contractAddress, string rpcUrl, bool updateTotalSupply = true)
            : this(chainId, new ERC20Service(() => rpcUrl, contractAddress), updateTotalSupply)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCacheRequest"/> class with specified chain ID, contract address and RPC URL factory.
        /// </summary>
        /// <param name="chainId">The block-chain chain ID.</param>
        /// <param name="contractAddress">The ERC20 token contract address.</param>
        /// <param name="rpcUrlFactory">A function that returns the RPC URL to interact with the block-chain.</param>
        /// <param name="updateTotalSupply">Optional. Indicates whether to update the total supply of the token in the cache. Defaults to <see langword="true"/>.</param>
        public GetCacheRequest(long chainId, EthereumAddress contractAddress, Func<string> rpcUrlFactory, bool updateTotalSupply = true)
            : this(chainId, new ERC20Service(rpcUrlFactory, contractAddress), updateTotalSupply)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCacheRequest"/> class with specified chain ID, contract address and asynchronous RPC URL factory.
        /// </summary>
        /// <param name="chainId">The block-chain chain ID.</param>
        /// <param name="contractAddress">The ERC20 token contract address.</param>
        /// <param name="rpcUrlFactoryAsync">A function that asynchronously returns the RPC URL to interact with the block-chain.</param>
        /// <param name="updateTotalSupply">Optional. Indicates whether to update the total supply of the token in the cache. Defaults to <see langword="true"/>.</param>
        public GetCacheRequest(long chainId, EthereumAddress contractAddress, Func<Task<string>> rpcUrlFactoryAsync, bool updateTotalSupply = true)
            : this(chainId, new ERC20Service(rpcUrlFactoryAsync, contractAddress), updateTotalSupply)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCacheRequest"/> class with a specified ERC20 service.
        /// </summary>
        /// <param name="chainId">The block-chain chain ID.</param>
        /// <param name="erc20Service">The <see cref="IERC20Service"/> providing access to the token's details.</param>
        /// <param name="updateTotalSupply">Optional. Indicates whether to update the total supply of the token in the cache. Defaults to <see langword="true"/>.</param>
        /// <remarks>
        /// This constructor allows for more flexibility by accepting an instance of an <see cref="IERC20Service"/>,
        /// enabling the use of customized or mock services for testing purposes.
        /// </remarks>
        public GetCacheRequest(long chainId, IERC20Service erc20Service, bool updateTotalSupply = true)
        {
            ChainId = chainId;
            ERC20Service = erc20Service;
            UpdateTotalSupply = updateTotalSupply;
        }
    }
}