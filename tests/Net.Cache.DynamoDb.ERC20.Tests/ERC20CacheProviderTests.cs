using System.Numerics;
using Moq;
using Xunit;
using FluentAssertions;
using Net.Cryptography.SHA256;
using Amazon.DynamoDBv2.DataModel;
using Net.Cache.DynamoDb.ERC20.RPC;
using Net.Cache.DynamoDb.ERC20.Models;

namespace Net.Cache.DynamoDb.ERC20.Tests;

public class ERC20CacheProviderTests
{
    private const long chainId = 56;
    private const string contractAddress = "0x0000000000000000000000000000000000000000";
    private const byte decimals = 18;
    private const string name = "Test";
    private const string symbol = "TST";
    private const long totalSupply = 5500000;
    private const long updatedTotalSupply = 5000000;
    private readonly string key = $"{chainId}-{contractAddress}".ToSha256();
    private readonly IERC20Service mockErc20Service;

    public ERC20CacheProviderTests()
    {
        Environment.SetEnvironmentVariable("AWS_REGION", "us-east-1");
        mockErc20Service = MockERC20Service();
    }

    [Fact]
    internal void GetOrAdd_ItemReceivedFromCache_TotalSupplyHasBeenUpdated()
    {
        var erc20StorageProvider = new ERC20StorageProvider(MockContext(true));
        var erc20CacheProvider = new ERC20CacheProvider(erc20StorageProvider);

        var addedItem = erc20CacheProvider.GetOrAdd(new GetCacheRequest(chainId, mockErc20Service));
        var updatedItem = erc20CacheProvider.GetOrAdd(new GetCacheRequest(chainId, mockErc20Service));

        addedItem.Should().BeEquivalentTo(new ERC20DynamoDbTable(
            chainId, contractAddress, name, symbol, decimals, 0.0000000000055m
        ));
        updatedItem.Should().BeEquivalentTo(new ERC20DynamoDbTable(
            chainId, contractAddress, name, symbol, decimals, 0.0000000000050m
        ));
    }

    [Fact]
    internal void GetOrAdd_ItemSavedToCache()
    {
        var erc20StorageProvider = new ERC20StorageProvider(MockContext(false));
        var erc20CacheProvider = new ERC20CacheProvider(erc20StorageProvider);

        var addedItem = erc20CacheProvider.GetOrAdd(new GetCacheRequest(chainId, mockErc20Service));

        addedItem.Should().BeEquivalentTo(new ERC20DynamoDbTable(
            chainId, contractAddress, name, symbol, decimals, 0.0000000000055m
        ));
    }

    private static IERC20Service MockERC20Service()
    {
        var mock = new Mock<IERC20Service>();
        mock.Setup(x => x.ContractAddress)
            .Returns(contractAddress);
        mock.Setup(x => x.Decimals())
            .Returns(decimals);
        mock.Setup(x => x.Name())
            .Returns(name);
        mock.Setup(x => x.Symbol())
            .Returns(symbol);
        mock.SetupSequence(x => x.TotalSupply())
            .Returns(totalSupply)
            .Returns(updatedTotalSupply);

        return mock.Object;
    }

    private IDynamoDBContext MockContext(bool setupLoad)
    {
        var mock = new Mock<IDynamoDBContext>();
        if (setupLoad)
        {
            var value = new ERC20DynamoDbTable(chainId, contractAddress, name, symbol, decimals, 0.0000000000055m);
            mock.SetupSequence(x => x.LoadAsync<ERC20DynamoDbTable>(key, It.IsAny<DynamoDBOperationConfig>(), default))
                .ReturnsAsync(value)
                .ReturnsAsync(value);
        }
        return mock.Object;
    }

    [Fact]
    internal void GetOrAdd_CovalentServiceIntegration_WorksCorrectly()
    {
        var expectedDecimals = (byte)6;
        var expectedName = "USD Coin";
        var expectedSymbol = "USDC";
        var expectedTotalSupply = new BigInteger(1000000);

        var mockCovalentService = new Mock<IERC20Service>();
        mockCovalentService.Setup(x => x.ContractAddress).Returns(contractAddress);
        mockCovalentService.Setup(x => x.Decimals()).Returns(expectedDecimals);
        mockCovalentService.Setup(x => x.Name()).Returns(expectedName);
        mockCovalentService.Setup(x => x.Symbol()).Returns(expectedSymbol);
        mockCovalentService.Setup(x => x.TotalSupply()).Returns(expectedTotalSupply);

        var erc20StorageProvider = new ERC20StorageProvider(MockContext(false));
        var erc20CacheProvider = new ERC20CacheProvider(erc20StorageProvider);

        var addedItem = erc20CacheProvider.GetOrAdd(new GetCacheRequest(chainId, mockCovalentService.Object));

        addedItem.Should().BeEquivalentTo(new ERC20DynamoDbTable(
            chainId, contractAddress, expectedName, expectedSymbol, expectedDecimals, expectedTotalSupply
        ));
    }
}