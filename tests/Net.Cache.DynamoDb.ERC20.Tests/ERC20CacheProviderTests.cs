using Moq;
using Xunit;
using FluentAssertions;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20.RPC;
using Net.Cache.DynamoDb.ERC20.Models;
using Net.Cache.DynamoDb.ERC20.Cryptography;
using Amazon.DynamoDBv2;

namespace Net.Cache.DynamoDb.ERC20.Tests;

public class ERC20CacheProviderTests
{
    [Fact]
    internal void GetOrAdd()
    {
        var chainId = 56;
        var contractAddress = EthereumAddress.ZeroAddress;
        byte decimals = 18;
        var name = "Test";
        var symbol = "TST";
        var totalSupply = 5500000;
        var key = $"{chainId}-{contractAddress}".ToSha256();
        var mockErc20Service = new Mock<IERC20Service>();
        mockErc20Service.Setup(x => x.Decimals())
            .Returns(decimals);
        mockErc20Service.Setup(x => x.Name())
            .Returns(name);
        mockErc20Service.Setup(x => x.Symbol())
            .Returns(symbol);
        mockErc20Service.Setup(x => x.TotalSupply())
            .Returns(totalSupply);
        mockErc20Service.Setup(x => x.ContractAddress)
            .Returns(contractAddress);
        var mockAmazonDynamoDB = new Mock<IAmazonDynamoDB>();
        var mockDynamoDbStorageProvider = new Mock<DynamoDbStorageProvider<string, ERC20DynamoDbTable>>(mockAmazonDynamoDB.Object);

        var erc20CacheProvider = new ERC20CacheProvider(mockDynamoDbStorageProvider.Object);

        var result = erc20CacheProvider.GetOrAdd(key, chainId, mockErc20Service.Object);

        result.Should().BeEquivalentTo(new ERC20DynamoDbTable(
            chainId, contractAddress, name, symbol, decimals, 0.0000000000055m
        ));
    }
}