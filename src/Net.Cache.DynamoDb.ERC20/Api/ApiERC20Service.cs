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
    /// <summary>
    /// The ApiERC20Service class provides functionality to interact with ERC20 tokens via an API.
    /// This service caches the token data to avoid redundant API calls.
    /// </summary>
    public class ApiERC20Service : IERC20Service
    {
        private readonly ApiERC20ServiceConfig _config;
        private readonly Lazy<ApiResponse> _tokenDataCache;

        /// <summary>
        /// Gets the contract address of the ERC20 token.
        /// </summary>
        public EthereumAddress ContractAddress => _config.ContractAddress;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiERC20Service"/> class using the provided configuration.
        /// </summary>
        /// <param name="config">The configuration containing API key, chain ID, contract address, and API URL.</param>
        public ApiERC20Service(ApiERC20ServiceConfig config)
        {
            _config = config;
            _tokenDataCache = new Lazy<ApiResponse>(() => LoadTokenData(_config.ApiUrl));
        }

        /// <summary>
        /// Loads the token data from the API and caches it.
        /// </summary>
        /// <param name="apiUrl">The API URL template to fetch the token data.</param>
        /// <returns>The token data as an <see cref="ApiResponse"/>.</returns>
        private ApiResponse LoadTokenData(string apiUrl)
        {
            var template = Handlebars.Compile(apiUrl);

            var url = template(new
            {
                chainId = _config.ChainId,
                contractAddress = _config.ContractAddress,
                apiKey = _config.ApiKey
            });

            var responseString = url.GetStringAsync().GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<ApiResponse>(responseString);
        }

        /// <summary>
        /// Gets the cached token data.
        /// </summary>
        /// <returns>The cached token data as an <see cref="ApiResponse"/>.</returns>
        public ApiResponse GetTokenData() => _tokenDataCache.Value;

        /// <summary>
        /// Retrieves the number of decimals the ERC20 token uses.
        /// </summary>
        /// <returns>The number of decimals.</returns>
        public byte Decimals() => GetTokenData().Data.Items[1].ContractDecimals;

        /// <summary>
        /// Retrieves the name of the ERC20 token.
        /// </summary>
        /// <returns>The name of the token.</returns>
        public string Name() => GetTokenData().Data.Items[1].ContractName;

        /// <summary>
        /// Retrieves the symbol of the ERC20 token.
        /// </summary>
        /// <returns>The symbol of the token.</returns>
        public string Symbol() => GetTokenData().Data.Items[1].ContractTickerSymbol;

        /// <summary>
        /// Retrieves the total supply of the ERC20 token.
        /// </summary>
        /// <returns>The total supply as a <see cref="BigInteger"/>.</returns>
        public BigInteger TotalSupply() => BigInteger.Parse(GetTokenData().Data.Items[1].TotalSupply);
    }
}