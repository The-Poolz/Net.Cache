using Moq;
using Xunit;
using System.Numerics;
using FluentAssertions;
using System.Reflection;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20.Rpc;
using System.Collections.Concurrent;
using Net.Cache.DynamoDb.ERC20.DynamoDb;
using Net.Cache.DynamoDb.ERC20.Rpc.Models;
using Net.Cache.DynamoDb.ERC20.DynamoDb.Models;

namespace Net.Cache.DynamoDb.ERC20.Tests;

public class Erc20CacheServiceTests
{
    public class Constructor
    {
        [Fact]
        public void Default()
        {
            Environment.SetEnvironmentVariable("AWS_REGION", "us-east-1");

            var cacheService = new Erc20CacheService();

            cacheService.Should().NotBeNull();
        }
    }

    public class GetOrAddAsync
    {
        [Fact]
        public async Task ReturnsEntryFromInMemoryCache_WhenPresent()
        {
            var dynamoDbClientMock = new Mock<IDynamoDbClient>(MockBehavior.Strict);
            var erc20FactoryMock = new Mock<IErc20ServiceFactory>(MockBehavior.Strict);
            var service = new Erc20CacheService(dynamoDbClientMock.Object, erc20FactoryMock.Object);

            var hashKey = new HashKey(1, EthereumAddress.ZeroAddress);
            var token = new Erc20TokenData(EthereumAddress.ZeroAddress, "Token", "TKN", 18, new BigInteger(1000));
            var entry = new Erc20TokenDynamoDbEntry(hashKey, token);

            var cacheField = typeof(Erc20CacheService).GetField("_inMemoryCache", BindingFlags.NonPublic | BindingFlags.Instance);
            var cache = (ConcurrentDictionary<string, Erc20TokenDynamoDbEntry>)cacheField!.GetValue(service)!;
            cache.TryAdd(hashKey.Value, entry);

            var rpcUrlFactoryMock = new Mock<Func<Task<string>>>(MockBehavior.Strict);
            var multiCallFactoryMock = new Mock<Func<Task<EthereumAddress>>>(MockBehavior.Strict);

            var result = await service.GetOrAddAsync(hashKey, rpcUrlFactoryMock.Object, multiCallFactoryMock.Object);

            result.Should().BeSameAs(entry);
        }

        [Fact]
        public async Task ReturnsEntryFromDynamoDb_WhenCacheMisses()
        {
            var hashKey = new HashKey(1, EthereumAddress.ZeroAddress);
            var token = new Erc20TokenData(EthereumAddress.ZeroAddress, "Token", "TKN", 18, new BigInteger(1000));
            var entry = new Erc20TokenDynamoDbEntry(hashKey, token);

            var dynamoDbClientMock = new Mock<IDynamoDbClient>(MockBehavior.Strict);
            dynamoDbClientMock
                .Setup(x => x.GetErc20TokenAsync(hashKey, null))
                .ReturnsAsync(entry);

            var erc20FactoryMock = new Mock<IErc20ServiceFactory>(MockBehavior.Strict);
            var service = new Erc20CacheService(dynamoDbClientMock.Object, erc20FactoryMock.Object);

            var rpcUrlFactoryMock = new Mock<Func<Task<string>>>(MockBehavior.Strict);
            var multiCallFactoryMock = new Mock<Func<Task<EthereumAddress>>>(MockBehavior.Strict);

            var result = await service.GetOrAddAsync(hashKey, rpcUrlFactoryMock.Object, multiCallFactoryMock.Object);

            result.Should().BeEquivalentTo(entry);

            var cacheField = typeof(Erc20CacheService).GetField("_inMemoryCache", BindingFlags.NonPublic | BindingFlags.Instance);
            var cache = (ConcurrentDictionary<string, Erc20TokenDynamoDbEntry>)cacheField!.GetValue(service)!;
            cache[hashKey.Value].Should().BeEquivalentTo(entry);

            dynamoDbClientMock.Verify(x => x.GetErc20TokenAsync(hashKey, null), Times.Once);
        }

        [Fact]
        public async Task FetchesFromRpcAndSaves_WhenNotInCacheOrDb()
        {
            var hashKey = new HashKey(1, EthereumAddress.ZeroAddress);
            var token = new Erc20TokenData(EthereumAddress.ZeroAddress, "Token", "TKN", 18, new BigInteger(1000));

            var dynamoDbClientMock = new Mock<IDynamoDbClient>(MockBehavior.Strict);
            dynamoDbClientMock
                .Setup(x => x.GetErc20TokenAsync(hashKey, null))
                .ReturnsAsync((Erc20TokenDynamoDbEntry?)null);
            dynamoDbClientMock
                .Setup(x => x.SaveErc20TokenAsync(It.Is<Erc20TokenDynamoDbEntry>(e => e.HashKey == hashKey.Value), null))
                .Returns(Task.CompletedTask);

            var erc20ServiceMock = new Mock<IErc20Service>(MockBehavior.Strict);
            erc20ServiceMock
                .Setup(x => x.GetErc20TokenAsync(EthereumAddress.ZeroAddress))
                .ReturnsAsync(token);

            var erc20FactoryMock = new Mock<IErc20ServiceFactory>(MockBehavior.Strict);
            var multiCall = new EthereumAddress("0x0000000000000000000000000000000000000001");
            erc20FactoryMock
                .Setup(x => x.Create(It.IsAny<Nethereum.Web3.IWeb3>(), multiCall))
                .Returns(erc20ServiceMock.Object);

            var service = new Erc20CacheService(dynamoDbClientMock.Object, erc20FactoryMock.Object);

            var rpcUrlFactoryMock = new Mock<Func<Task<string>>>();
            rpcUrlFactoryMock.Setup(f => f()).ReturnsAsync("https://rpc");
            var multiCallFactoryMock = new Mock<Func<Task<EthereumAddress>>>();
            multiCallFactoryMock.Setup(f => f()).ReturnsAsync(multiCall);

            var result = await service.GetOrAddAsync(hashKey, rpcUrlFactoryMock.Object, multiCallFactoryMock.Object);

            result.HashKey.Should().Be(hashKey.Value);
            result.Name.Should().Be("Token");

            dynamoDbClientMock.Verify(x => x.GetErc20TokenAsync(hashKey, null), Times.Once);
            dynamoDbClientMock.Verify(x => x.SaveErc20TokenAsync(It.IsAny<Erc20TokenDynamoDbEntry>(), null), Times.Once);
            erc20FactoryMock.Verify(x => x.Create(It.IsAny<Nethereum.Web3.IWeb3>(), multiCall), Times.Once);
            erc20ServiceMock.Verify(x => x.GetErc20TokenAsync(EthereumAddress.ZeroAddress), Times.Once);
            rpcUrlFactoryMock.Verify(f => f(), Times.Once);
            multiCallFactoryMock.Verify(f => f(), Times.Once);
        }
    }
}