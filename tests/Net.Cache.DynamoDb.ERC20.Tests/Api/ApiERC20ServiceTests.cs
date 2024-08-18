using Xunit;
using System.Numerics;
using FluentAssertions;
using Flurl.Http.Testing;
using Newtonsoft.Json.Linq;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20.Api;
using Net.Cache.DynamoDb.ERC20.Models;
using Net.Cache.DynamoDb.ERC20.Models.Api;

namespace Net.Cache.DynamoDb.ERC20.Tests.Api;

public class ApiERC20ServiceTests
{
    private readonly long _chainId = 1;
    private readonly string _apiKey = "test-api-key";
    private readonly ApiERC20Service _apiErc20Service;
    private readonly string _apiUrl = "https://api.covalenthq.com/v1/{{chainId}}/tokens/{{contractAddress}}/token_holders_v2/?page-size=100&page-number=0&key={{apiKey}}";
    private readonly string _contractAddress;

    public ApiERC20ServiceTests()
    {
        _contractAddress = EthereumAddress.ZeroAddress;
        _apiErc20Service = new ApiERC20Service(new ApiERC20ServiceConfig(_apiKey, _chainId, _contractAddress, _apiUrl));
    }

    private static JObject CreateMockResponse(byte decimals, string name, string symbol, string totalSupply)
    {
        return new JObject
        {
            ["data"] = new JObject
            {
                ["items"] = new JArray
                {
                    new JObject
                    {
                        ["contract_decimals"] = decimals + 1,
                        ["contract_name"] = name + " 2",
                        ["contract_ticker_symbol"] = symbol + "2",
                        ["total_supply"] = totalSupply
                    },
                    new JObject
                    {
                        ["contract_decimals"] = decimals,
                        ["contract_name"] = name,
                        ["contract_ticker_symbol"] = symbol,
                        ["total_supply"] = totalSupply
                    }
                }
            }
        };
    }

    [Fact]
    public void ApiERC20Service_Methods_ShouldReturnExpectedValues()
    {
        using var httpTest = new HttpTest();
        const byte expectedDecimals = 18;
        const string expectedName = "Test Token";
        const string expectedSymbol = "TTT";
        const string expectedTotalSupply = "1000000";
        var jsonResponse = CreateMockResponse(expectedDecimals, expectedName, expectedSymbol, expectedTotalSupply);

        httpTest.RespondWith(jsonResponse.ToString());

        var decimals = _apiErc20Service.Decimals();
        var name = _apiErc20Service.Name();
        var symbol = _apiErc20Service.Symbol();
        var totalSupply = _apiErc20Service.TotalSupply();

        decimals.Should().Be(expectedDecimals);
        name.Should().Be(expectedName);
        symbol.Should().Be(expectedSymbol);
        totalSupply.Should().Be(BigInteger.Parse(expectedTotalSupply));
    }

    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        var ethereumAddress = (EthereumAddress)_contractAddress;

        var cacheRequest = GetCacheRequest.CreateWithCovalentService(_apiKey, _chainId, ethereumAddress, _apiUrl);

        cacheRequest.ChainId.Should().Be(_chainId);
        cacheRequest.ERC20Service.Should().NotBeNull();
        cacheRequest.ERC20Service.ContractAddress.Should().Be(ethereumAddress);
    }

    [Fact]
    public async Task GetTokenData_ShouldReturnCorrectData()
    {
        using var httpTest = new HttpTest();
        var expectedJson = CreateMockResponse(18, "Test Token", "TTT", "1000000");

        httpTest.RespondWith(expectedJson.ToString());

        var tokenData = _apiErc20Service.GetTokenData();

        var result = JObject.FromObject(tokenData);

        result.Should().BeEquivalentTo(expectedJson);

        var expectedUrl = $"https://api.covalenthq.com/v1/1/tokens/0x0000000000000000000000000000000000000000/token_holders_v2/?page-size=100&page-number=0&key=test-api-key";

        httpTest.ShouldHaveCalled(expectedUrl)
            .WithVerb(HttpMethod.Get);
    }


    [Fact]
    public void Constructor_ShouldInitializeFieldsCorrectly()
    {
        var service = new ApiERC20Service(new ApiERC20ServiceConfig(_apiKey, _chainId, _contractAddress, _apiUrl));

        service.ContractAddress.Should().Be((EthereumAddress)_contractAddress);
        service.Should().NotBeNull();
    }
}