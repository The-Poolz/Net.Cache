using System;
using System.Threading.Tasks;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20.Rpc;
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
            _dynamoDbClient = dynamoDbClient ?? throw new ArgumentNullException(nameof(dynamoDbClient));
            _erc20ServiceFactory = erc20ServiceFactory ?? throw new ArgumentNullException(nameof(erc20ServiceFactory));
        }

        public Erc20CacheService()
            : this(
                new DynamoDbClient(),
                new Erc20ServiceFactory()
            )
        { }

        public async Task<Erc20TokenDynamoDbEntry> GetOrAddAsync(long chainId, EthereumAddress address, Func<Task<string>> rpcUrlFactory, Func<Task<EthereumAddress>> multiCallFactory)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));
            if (rpcUrlFactory == null) throw new ArgumentNullException(nameof(rpcUrlFactory));
            if (multiCallFactory == null) throw new ArgumentNullException(nameof(multiCallFactory));

            var entry = await _dynamoDbClient
                .GetErc20TokenAsync(Erc20TokenDynamoDbEntry.GenerateHashKey(chainId, address))
                .ConfigureAwait(false);
            if (entry != null) return entry;

            var rpcUrlTask = rpcUrlFactory();
            var multiCallTask = multiCallFactory();

            await Task.WhenAll(rpcUrlTask, multiCallTask);

            var rpcUrl = rpcUrlTask.Result;
            var multiCall = multiCallTask.Result;

            var erc20Service = _erc20ServiceFactory.Create(new Nethereum.Web3.Web3(rpcUrl), multiCall);
            var erc20Token = await erc20Service.GetErc20TokenAsync(address).ConfigureAwait(false);

            entry = new Erc20TokenDynamoDbEntry(chainId, address, erc20Token);
            await _dynamoDbClient.SaveErc20TokenAsync(entry).ConfigureAwait(false);

            return entry;
        }
    }
}