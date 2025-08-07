using Xunit;
using FluentAssertions;
using Net.Cryptography.SHA256;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20.DynamoDb.Models;

namespace Net.Cache.DynamoDb.ERC20.Tests.DynamoDb.Models;

public class HashKeyTests
{
    public const long chainId = 1;
    public const string address = EthereumAddress.ZeroAddress;

    public class Constructor
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void WhenChainIdIsInvalid_ShouldThrow(long chainId)
        {
            var act = () => new HashKey(chainId, address);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void WhenAddressIsNull_ShouldThrow()
        {
            var act = () => new HashKey(1, null!);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ShouldInitializeProperties()
        {
            var expected = $"{chainId}-{address}".ToSha256();

            var key = new HashKey(chainId, address);

            key.ChainId.Should().Be(chainId);
            key.Address.Should().Be(address);
            key.Value.Should().Be(expected);
            key.ToString().Should().Be(expected);
        }
    }

    public class Generate
    {
        [Fact]
        public void ShouldReturnSha256Value()
        {
            var expected = $"{chainId}-{address}".ToSha256();

            var value = HashKey.Generate(chainId, address);

            value.Should().Be(expected);
        }
    }
}