using System.Numerics;
using Xunit;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20.Covalent;
using Flurl.Http.Testing;
using Amazon.DynamoDBv2.DataModel;
using Moq;
using Net.Cache.DynamoDb.ERC20.Models;
using Net.Cryptography.SHA256;
using Net.Cache.DynamoDb.ERC20.RPC;

namespace Net.Cache.DynamoDb.ERC20.Tests.Covalent
{
    public class CovalentServiceTests
    {
        private readonly long chainId = 1;
        private readonly string apiKey = "test-api-key";
        private readonly CovalentService covalentService;
        private const string ContractAddress = "0x";

        public CovalentServiceTests()
        {
            var contractAddress = EthereumAddress.ZeroAddress;
            covalentService = new CovalentService(apiKey, chainId, contractAddress);
        }

        [Fact]
        public async Task DecimalsAsync_ShouldReturnExpectedDecimals()
        {
            using (var httpTest = new HttpTest())
            {
                byte expectedDecimals = 18;
                var jsonResponse = new JObject
                {
                    ["data"] = new JObject
                    {
                        ["items"] = new JArray
                        {
                            new JObject
                            {
                                ["contract_decimals"] = expectedDecimals,
                                ["contract_name"] = "Test Token",
                                ["contract_ticker_symbol"] = "TTT"
                            }
                        }
                    }
                };

                httpTest.RespondWith(jsonResponse.ToString());

                var decimals = await covalentService.DecimalsAsync();
                var name = await covalentService.NameAsync();
                var symbol = await covalentService.SymbolAsync();

                decimals.Should().Be(expectedDecimals);
                name.Should().Be("Test Token");
                symbol.Should().Be("TTT");

                (await covalentService.DecimalsAsync()).Should().Be(expectedDecimals);
                covalentService.Name().Should().Be("Test Token");
                covalentService.Symbol().Should().Be("TTT");
            }
        }

        [Fact]
        public async Task GetTokenDataAsync_ShouldReturnExpectedData()
        {
            using (var httpTest = new HttpTest())
            {
                var expectedData = new JObject
                {
                    ["data"] = new JObject
                    {
                        ["items"] = new JArray
                        {
                            new JObject
                            {
                                ["contract_decimals"] = 18,
                                ["contract_name"] = "TestToken",
                                ["contract_ticker_symbol"] = "TT"
                            }
                        }
                    }
                };

                httpTest.RespondWith(expectedData.ToString());

                var result = await covalentService.GetTokenDataAsync();

                Assert.NotNull(result);
                Assert.Equal(expectedData.ToString(), result.ToString());
            }
        }
    }
}