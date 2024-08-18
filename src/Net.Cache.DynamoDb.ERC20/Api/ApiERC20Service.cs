using System;
using Flurl.Http;
using System.Numerics;
using Newtonsoft.Json;
using HandlebarsDotNet;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20.RPC;
using Net.Cache.DynamoDb.ERC20.Models.Api;

namespace Net.Cache.DynamoDb.ERC20.Api
{
    public class ApiERC20Service : IERC20Service
    {
        private readonly string _apiKey;
        private readonly long _chainId;
        private readonly EthereumAddress _contractAddress;
        private readonly Lazy<ApiResponse> _tokenDataCache;

        public EthereumAddress ContractAddress => _contractAddress;

        public ApiERC20Service(string apiKey, long chainId, EthereumAddress contractAddress, string apiUrl)
        {
            _apiKey = apiKey;
            _chainId = chainId;
            _contractAddress = contractAddress;
            _tokenDataCache = new Lazy<ApiResponse>(() => LoadTokenData(apiUrl));
        }

        private ApiResponse LoadTokenData(string apiUrl)
        {
            var template = Handlebars.Compile(apiUrl);

            var url = template(new
            {
                chainId = _chainId,
                contractAddress = _contractAddress.ToString(),
                apiKey = _apiKey
            });

            var responseString = url.GetStringAsync().GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<ApiResponse>(responseString);
        }

        public ApiResponse GetTokenData() => _tokenDataCache.Value;

        public byte Decimals()
        {
            var tokenData = GetTokenData();
            return tokenData.Data.Items[1].ContractDecimals;
        }

        public string Name()
        {
            var tokenData = GetTokenData();
            return tokenData.Data.Items[1].ContractName;
        }

        public string Symbol()
        {
            var tokenData = GetTokenData();
            return tokenData.Data.Items[1].ContractTickerSymbol;
        }

        public BigInteger TotalSupply()
        {
            var tokenData = GetTokenData();
            return BigInteger.Parse(tokenData.Data.Items[1].TotalSupply);
        }
    }
}