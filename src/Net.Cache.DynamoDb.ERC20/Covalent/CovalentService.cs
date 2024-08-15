using System;
using Flurl.Http;
using Newtonsoft.Json;
using System.Numerics;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20.RPC;

namespace Net.Cache.DynamoDb.ERC20.Covalent
{
    public class CovalentService : IERC20Service
    {
        private readonly long _chainId;
        private readonly EthereumAddress _contractAddress;
        private readonly string _apiKey;
        private readonly Lazy<Task<JObject>> _cachedTokenData;

        public EthereumAddress ContractAddress => _contractAddress;

        public CovalentService(long chainId, EthereumAddress contractAddress, string apiKey)
        {
            _apiKey = apiKey;
            _contractAddress = contractAddress;
            _chainId = chainId;
            _cachedTokenData = new Lazy<Task<JObject>>(LoadTokenDataAsync);
        }

        private async Task<JObject> LoadTokenDataAsync()
        {
            var url = $"https://api.covalenthq.com/v1/{_chainId}/tokens/{_contractAddress}/token_holders_v2/?" + $"page-size=100&page-number=0&key={_apiKey}";

            var responseString = await url.GetStringAsync();

            return JsonConvert.DeserializeObject<JObject>(responseString);
        }

        public Task<JObject> GetTokenDataAsync() => _cachedTokenData.Value;

        public async Task<byte> DecimalsAsync()
        {
            var tokenData = await GetTokenDataAsync();
            return tokenData["data"]["items"][0]["contract_decimals"].Value<byte>();
        }

        public async Task<string> NameAsync()
        {
            var tokenData = await GetTokenDataAsync();
            return tokenData["data"]["items"][0]["contract_name"].Value<string>();
        }

        public async Task<string> SymbolAsync()
        {
            var tokenData = await GetTokenDataAsync();
            return tokenData["data"]["items"][0]["contract_ticker_symbol"].Value<string>();
        }

        public async Task<BigInteger> TotalSupplyAsync()
        {
            var tokenData = await GetTokenDataAsync();
            var totalSupplyString = tokenData["data"]["items"][0]["total_supply"].Value<string>();

            return BigInteger.Parse(totalSupplyString);
        }

        public byte Decimals() => DecimalsAsync().GetAwaiter().GetResult();
        public string Name() => NameAsync().GetAwaiter().GetResult();
        public string Symbol() => SymbolAsync().GetAwaiter().GetResult();
        public BigInteger TotalSupply() => TotalSupplyAsync().GetAwaiter().GetResult();
    }
}