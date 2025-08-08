using Xunit;
using System.Text;
using System.Numerics;
using FluentAssertions;
using Nethereum.Hex.HexConvertors.Extensions;
using Net.Cache.DynamoDb.ERC20.Rpc.Extensions;
using Nethereum.Contracts.Standards.ERC20.ContractDefinition;

namespace Net.Cache.DynamoDb.ERC20.Tests.Rpc.Extensions
{
    public class DecoderExtensionsTests
    {
        [Fact]
        public void ShouldDecodeErc20Outputs()
        {
            var nameData = EncodeString("TokenName");
            var symbolData = EncodeString("TN");
            var decimalsData = EncodeNumber(18);
            var supplyData = EncodeNumber(new BigInteger(1000));

            var responses = new[] { nameData, symbolData, decimalsData, supplyData };

            responses[0].Decode<NameOutputDTO>().Name.Should().Be("TokenName");
            responses[1].Decode<SymbolOutputDTO>().Symbol.Should().Be("TN");
            responses[2].Decode<DecimalsOutputDTO>().Decimals.Should().Be(18);
            responses[3].Decode<TotalSupplyOutputDTO>().TotalSupply.Should().Be(new BigInteger(1000));
        }

        private static byte[] EncodeString(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            var lengthHex = bytes.Length.ToString("x").PadLeft(64, '0');
            var dataHex = bytes.ToHex().PadRight(((bytes.Length + 31) / 32) * 64, '0');
            var hex = "0000000000000000000000000000000000000000000000000000000000000020" + lengthHex + dataHex;
            return hex.HexToByteArray();
        }

        private static byte[] EncodeNumber(BigInteger value)
        {
            return value.ToString("x").PadLeft(64, '0').HexToByteArray();
        }
    }
}
