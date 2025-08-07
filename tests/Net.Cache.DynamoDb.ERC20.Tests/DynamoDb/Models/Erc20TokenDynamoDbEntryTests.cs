using Xunit;
using System.Numerics;
using FluentAssertions;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20.Rpc.Models;
using Net.Cache.DynamoDb.ERC20.DynamoDb.Models;

namespace Net.Cache.DynamoDb.ERC20.Tests.DynamoDb.Models
{
    public class Erc20TokenDynamoDbEntryTests
    {
        [Fact]
        public void Constructor_ShouldMapFields()
        {
            var address = new EthereumAddress("0x0000000000000000000000000000000000000001");
            var chainId = 1;
            var hashKey = new HashKey(chainId, address);
            var token = new Erc20TokenData(address, "Token", "TKN", 2, new BigInteger(1000));

            var entry = new Erc20TokenDynamoDbEntry(hashKey, token);

            entry.HashKey.Should().Be(hashKey.Value);
            entry.ChainId.Should().Be(chainId);
            entry.Address.Should().Be(address);
            entry.Name.Should().Be("Token");
            entry.Symbol.Should().Be("TKN");
            entry.Decimals.Should().Be(2);
            entry.TotalSupply.Should().Be(10m);
        }
    }
}
