using Net.Web3.EthereumWallet;

namespace Net.Cache.DynamoDb.ERC20.Models.Api
{
    /// <summary>
    /// Represents the configuration settings required for the API-based ERC20 service.
    /// </summary>
    public class ApiERC20ServiceConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiERC20ServiceConfig"/> class with the specified settings.
        /// </summary>
        /// <param name="apiKey">The API key used to authenticate requests to the ERC20 token service.</param>
        /// <param name="chainId">The blockchain chain ID where the ERC20 token resides.</param>
        /// <param name="contractAddress">The Ethereum address of the ERC20 token contract.</param>
        /// <param name="apiUrl">The URL template for making API requests to retrieve ERC20 token information.</param>
        public ApiERC20ServiceConfig(string apiKey, long chainId, EthereumAddress contractAddress, string apiUrl)
        {
            ApiKey = apiKey;
            ChainId = chainId;
            ContractAddress = contractAddress;
            ApiUrl = apiUrl;
        }

        /// <summary>
        /// Gets or sets the API key used to authenticate requests to the ERC20 token service.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the blockchain chain ID where the ERC20 token resides.
        /// </summary>
        public long ChainId { get; set; }

        /// <summary>
        /// Gets or sets the Ethereum address of the ERC20 token contract.
        /// </summary>
        public EthereumAddress ContractAddress { get; set; }

        /// <summary>
        /// Gets or sets the URL template for making API requests to retrieve ERC20 token information.
        /// </summary>
        public string ApiUrl { get; set; }
    }
}