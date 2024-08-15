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
    /// <summary>
    /// Provides functionality to interact with ERC20 tokens using the Covalent API service.
    /// </summary>
    /// <remarks>
    /// The <see cref="CovalentService"/> class implements the <see cref="IERC20Service"/> interface to allow 
    /// interactions with an ERC20 token contract via the Covalent API. This class caches the token data 
    /// to minimize the number of API calls.
    /// </remarks>
    public class CovalentService : IERC20Service
    {
        private readonly string _apiKey;
        private readonly long _chainId;
        private readonly EthereumAddress _contractAddress;
        private readonly Lazy<Task<JObject>> _cachedTokenData;

        /// <summary>
        /// Gets the contract address of the ERC20 token.
        /// </summary>
        public EthereumAddress ContractAddress => _contractAddress;

        /// <summary>
        /// Initializes a new instance of the <see cref="CovalentService"/> class with specified API key, 
        /// chain ID, and contract address.
        /// </summary>
        /// <param name="apiKey">The API key for accessing the Covalent API service.</param>
        /// <param name="chainId">The blockchain chain ID.</param>
        /// <param name="contractAddress">The Ethereum address of the ERC20 token contract.</param>
        /// <remarks>
        /// This constructor creates an instance of the <see cref="CovalentService"/> class using the provided API key, 
        /// chain ID, and contract address, and initializes a lazy-loaded cache for the token data.
        /// </remarks>
        public CovalentService(string apiKey, long chainId, EthereumAddress contractAddress)
        {
            _apiKey = apiKey;
            _chainId = chainId;
            _contractAddress = contractAddress;
            _cachedTokenData = new Lazy<Task<JObject>>(LoadTokenDataAsync);
        }

        /// <summary>
        /// Asynchronously loads the token data from the Covalent API.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the token data as a <see cref="JObject"/>.</returns>
        /// <remarks>
        /// This method constructs the API URL using the provided parameters and retrieves the token data. 
        /// The data is cached to minimize subsequent API calls.
        /// </remarks>
        private async Task<JObject> LoadTokenDataAsync()
        {
            var url = $"https://api.covalenthq.com/v1/{_chainId}/tokens/{_contractAddress}/token_holders_v2/?" + $"page-size=100&page-number=0&key={_apiKey}";

            var responseString = await url.GetStringAsync();
            return JsonConvert.DeserializeObject<JObject>(responseString);
        }

        /// <summary>
        /// Gets the cached token data.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the token data as a <see cref="JObject"/>.</returns>
        public Task<JObject> GetTokenDataAsync() => _cachedTokenData.Value;

        /// <summary>
        /// Asynchronously retrieves the number of decimals the ERC20 token uses.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of decimals as a <see cref="byte"/>.</returns>
        public async Task<byte> DecimalsAsync()
        {
            var tokenData = await GetTokenDataAsync();
            return tokenData["data"]["items"][1]["contract_decimals"].Value<byte>();
        }

        /// <summary>
        /// Asynchronously retrieves the name of the ERC20 token.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the name of the token as a <see cref="string"/>.</returns>
        public async Task<string> NameAsync()
        {
            var tokenData = await GetTokenDataAsync();
            return tokenData["data"]["items"][1]["contract_name"].Value<string>();
        }

        /// <summary>
        /// Asynchronously retrieves the symbol of the ERC20 token.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the symbol of the token as a <see cref="string"/>.</returns>
        public async Task<string> SymbolAsync()
        {
            var tokenData = await GetTokenDataAsync();
            return tokenData["data"]["items"][1]["contract_ticker_symbol"].Value<string>();
        }

        /// <summary>
        /// Asynchronously retrieves the total supply of the ERC20 token.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the total supply of the token as a <see cref="BigInteger"/>.</returns>
        public async Task<BigInteger> TotalSupplyAsync()
        {
            var tokenData = await GetTokenDataAsync();
            var totalSupplyString = tokenData["data"]["items"][1]["total_supply"].Value<string>();

            return BigInteger.Parse(totalSupplyString);
        }

        /// <inheritdoc/>
        public byte Decimals() => DecimalsAsync().GetAwaiter().GetResult();

        /// <inheritdoc/>
        public string Name() => NameAsync().GetAwaiter().GetResult();

        /// <inheritdoc/>
        public string Symbol() => SymbolAsync().GetAwaiter().GetResult();

        /// <inheritdoc/>
        public BigInteger TotalSupply() => TotalSupplyAsync().GetAwaiter().GetResult();
    }
}