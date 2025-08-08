using System;
using System.Threading.Tasks;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20.DynamoDb.Models;

namespace Net.Cache.DynamoDb.ERC20
{
    /// <summary>
    /// Provides caching operations for ERC20 token metadata backed by DynamoDB.
    /// </summary>
    public interface IErc20CacheService
    {
        /// <summary>
        /// Retrieves a cached ERC20 token entry or adds a new one when it is missing.
        /// </summary>
        /// <param name="hashKey">The composite key of chain identifier and token address.</param>
        /// <param name="rpcUrlFactory">Factory used to resolve the RPC endpoint URL.</param>
        /// <param name="multiCallFactory">Factory used to resolve the address of the multicall contract.</param>
        /// <returns>The cached or newly created <see cref="Erc20TokenDynamoDbEntry"/> instance.</returns>
        public Task<Erc20TokenDynamoDbEntry> GetOrAddAsync(HashKey hashKey, Func<Task<string>> rpcUrlFactory, Func<Task<EthereumAddress>> multiCallFactory);
    }
}
