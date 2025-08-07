using System;
using System.Threading.Tasks;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20.Rpc;
using System.Collections.Concurrent;
using Net.Cache.DynamoDb.ERC20.DynamoDb;
using Net.Cache.DynamoDb.ERC20.DynamoDb.Models;

namespace Net.Cache.DynamoDb.ERC20
{
    /// <summary>
    /// Provides caching of ERC20 token information with a backing store in DynamoDB and an in-memory layer.
    /// </summary>
    public class Erc20CacheService : IErc20CacheService
    {
        private readonly IDynamoDbClient _dynamoDbClient;
        private readonly IErc20ServiceFactory _erc20ServiceFactory;
        private readonly ConcurrentDictionary<string, Erc20TokenDynamoDbEntry> _inMemoryCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="Erc20CacheService"/> class.
        /// </summary>
        /// <param name="dynamoDbClient">The DynamoDB client used for persistent storage.</param>
        /// <param name="erc20ServiceFactory">Factory for creating RPC services to query token metadata.</param>
        public Erc20CacheService(IDynamoDbClient dynamoDbClient, IErc20ServiceFactory erc20ServiceFactory)
        {
            _dynamoDbClient = dynamoDbClient ?? throw new ArgumentNullException(nameof(dynamoDbClient));
            _erc20ServiceFactory = erc20ServiceFactory ?? throw new ArgumentNullException(nameof(erc20ServiceFactory));
            _inMemoryCache = new ConcurrentDictionary<string, Erc20TokenDynamoDbEntry>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Erc20CacheService"/> class using default implementations.
        /// </summary>
        public Erc20CacheService()
            : this(
                new DynamoDbClient(),
                new Erc20ServiceFactory()
            )
        { }

        /// <inheritdoc cref="IErc20CacheService.GetOrAddAsync"/>
        public async Task<Erc20TokenDynamoDbEntry> GetOrAddAsync(HashKey hashKey, Func<Task<string>> rpcUrlFactory, Func<Task<EthereumAddress>> multiCallFactory)
        {
            if (hashKey == null) throw new ArgumentNullException(nameof(hashKey));
            if (rpcUrlFactory == null) throw new ArgumentNullException(nameof(rpcUrlFactory));
            if (multiCallFactory == null) throw new ArgumentNullException(nameof(multiCallFactory));

            if (_inMemoryCache.TryGetValue(hashKey.Value, out var cachedEntry)) return cachedEntry;

            var entry = await _dynamoDbClient
                .GetErc20TokenAsync(hashKey)
                .ConfigureAwait(false);
            if (entry != null)
            {
                _inMemoryCache.TryAdd(hashKey.Value, entry);
                return entry;
            }

            var rpcUrlTask = rpcUrlFactory();
            var multiCallTask = multiCallFactory();

            await Task.WhenAll(rpcUrlTask, multiCallTask);

            var rpcUrl = rpcUrlTask.Result;
            var multiCall = multiCallTask.Result;

            var erc20Service = _erc20ServiceFactory.Create(new Nethereum.Web3.Web3(rpcUrl), multiCall);
            var erc20Token = await erc20Service.GetErc20TokenAsync(hashKey.Address).ConfigureAwait(false);

            entry = new Erc20TokenDynamoDbEntry(hashKey, erc20Token);
            await _dynamoDbClient.SaveErc20TokenAsync(entry).ConfigureAwait(false);
            _inMemoryCache.TryAdd(hashKey.Value, entry);

            return entry;
        }
    }
}