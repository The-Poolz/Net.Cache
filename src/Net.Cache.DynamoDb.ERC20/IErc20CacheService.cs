using System;
using System.Threading.Tasks;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20.DynamoDb.Models;

namespace Net.Cache.DynamoDb.ERC20
{
    public interface IErc20CacheService
    {
        public Task<Erc20TokenDynamoDbEntry> GetOrAddAsync(long chainId, EthereumAddress address, Func<Task<string>> rpcUrlFactoryAsync, Func<Task<EthereumAddress>> multiCallFactoryAsync);
    }
}
