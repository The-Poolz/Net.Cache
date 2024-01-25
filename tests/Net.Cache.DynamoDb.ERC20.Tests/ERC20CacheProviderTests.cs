using Moq;
using Xunit;
using FluentAssertions;
using Net.Cache.DynamoDb.ERC20.RPC;
using Net.Cache.DynamoDb.ERC20.Models;
using Net.Cache.DynamoDb.ERC20.Cryptography;

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
    private readonly ERC20CacheProvider erc20CacheProvider;

    public ERC20CacheProviderTests()
    {
        Environment.SetEnvironmentVariable("AWS_REGION", "us-east-1");
        mockErc20Service = MockERC20Service();
        erc20CacheProvider = new ERC20CacheProvider();
    }

    [Fact]
    internal void GetOrAdd_ItemAddedToCache()
    {
        var result = erc20CacheProvider.GetOrAdd(key, new GetCacheRequest(chainId, mockErc20Service));

        result.Should().BeEquivalentTo(new ERC20DynamoDbTable(
            chainId, contractAddress, name, symbol, decimals, 0.0000000000055m
        ));
    }

    [Fact]
    internal void GetOrAdd_ItemReceivedFromCache_TotalSupplyHasBeenUpdated()
    {
        var addedItem = erc20CacheProvider.GetOrAdd(key, new GetCacheRequest(chainId, mockErc20Service));
        var updatedItem = erc20CacheProvider.GetOrAdd(key, new GetCacheRequest(chainId, mockErc20Service));

        addedItem.Should().BeEquivalentTo(new ERC20DynamoDbTable(
            chainId, contractAddress, name, symbol, decimals, 0.0000000000055m
        ));
        updatedItem.Should().BeEquivalentTo(new ERC20DynamoDbTable(
            chainId, contractAddress, name, symbol, decimals, 0.0000000000050m
        ));
    }

    private IERC20Service MockERC20Service()
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
}