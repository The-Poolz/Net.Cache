using System;
using System.Threading.Tasks;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20.RPC;
using Net.Cache.DynamoDb.ERC20.DynamoDb;
using Net.Cache.DynamoDb.ERC20.DynamoDb.Models;

namespace Net.Cache.DynamoDb.ERC20
{
    public class Erc20CacheService : IErc20CacheService
    {
        private readonly IDynamoDbClient _dynamoDbClient;
        private readonly IErc20ServiceFactory _erc20ServiceFactory;

        public Erc20CacheService(IDynamoDbClient dynamoDbClient, IErc20ServiceFactory erc20ServiceFactory)
        {
            _dynamoDbClient = dynamoDbClient;
            _erc20ServiceFactory = erc20ServiceFactory;
        }

        public Erc20CacheService()
            : this(
                new DynamoDbClient(),
                new Erc20ServiceFactory()
            )
        { }

        public async Task<Erc20TokenDynamoDbEntry> GetOrAddAsync(long chainId, EthereumAddress address, Func<Task<string>> rpcUrlFactoryAsync, Func<Task<EthereumAddress>> multiCallFactoryAsync)
        {
            var value = await _dynamoDbClient.GetErc20TokenAsync(Erc20TokenDynamoDbEntry.GenerateHashKey(chainId, address));
            if (value != null) return value;

            var rpcUrl = await rpcUrlFactoryAsync();
            var multiCall = await multiCallFactoryAsync();

            var erc20Service = _erc20ServiceFactory.Create(new Nethereum.Web3.Web3(rpcUrl), multiCall);
            var erc20Token = await erc20Service.GetEr20TokenAsync(address);

            value = new Erc20TokenDynamoDbEntry(chainId, address, erc20Token);
            await _dynamoDbClient.SaveErc20TokenAsync(value);

            return value;
        }
    }
}