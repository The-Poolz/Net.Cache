using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20.RPC;
using Net.Cache.DynamoDb.ERC20.Covalent;

namespace Net.Cache.DynamoDb.ERC20.Models
{
    /// <summary>
    /// Represents a request to retrieve or update ERC20 token information in the cache.
    /// </summary>
    /// <remarks>
    /// This class encapsulates the details necessary for fetching or updating the cached data of an ERC20 token,
    /// including the block-chain chain ID, the <see cref="IERC20Service"/>, and a flag indicating whether to update the total supply.
    /// </remarks>
    public class GetCacheRequest
    {
        /// <summary>
        /// Gets the block-chain chain ID for the request.
        /// </summary>
        public long ChainId { get; }

        /// <summary>
        /// Gets the <see cref="IERC20Service"/> used to interact with the ERC20 token contract.
        /// </summary>
        public IERC20Service ERC20Service { get; }

        /// <summary>
        /// Gets a value indicating whether the total supply of the token should be updated in the cache.
        /// </summary>
        public bool UpdateTotalSupply { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCacheRequest"/> class with specified chain ID and contract address.
        /// </summary>
        /// <param name="chainId">The block-chain chain ID.</param>
        /// <param name="contractAddress">The ERC20 token contract address.</param>
        /// <param name="rpcUrl">The URL of the RPC endpoint to interact with the block-chain.</param>
        /// <param name="updateTotalSupply">Optional. Indicates whether to update the total supply of the token in the cache. Defaults to <see langword="true"/>.</param>
        /// <remarks>
        /// This constructor creates an instance of the <see cref="ERC20Service"/> class using the provided RPC URL and contract address.
        /// </remarks>
        public GetCacheRequest(long chainId, EthereumAddress contractAddress, string rpcUrl, bool updateTotalSupply = true)
            : this(chainId, new ERC20Service(rpcUrl, contractAddress), updateTotalSupply)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCacheRequest"/> class with a specified ERC20 service.
        /// </summary>
        /// <param name="chainId">The block-chain chain ID.</param>
        /// <param name="erc20Service">The <see cref="IERC20Service"/> providing access to the token's details.</param>
        /// <param name="updateTotalSupply">Optional. Indicates whether to update the total supply of the token in the cache. Defaults to <see langword="true"/>.</param>
        /// <remarks>
        /// This constructor allows for more flexibility by accepting an instance of an <see cref="IERC20Service"/>,
        /// enabling the use of customized or mock services for testing purposes.
        /// </remarks>
        public GetCacheRequest(long chainId, IERC20Service erc20Service, bool updateTotalSupply = true)
        {
            ChainId = chainId;
            ERC20Service = erc20Service;
            UpdateTotalSupply = updateTotalSupply;
        }

        /// <summary>
        /// Creates an instance of the <see cref="GetCacheRequest"/> class using the Covalent API service.
        /// </summary>
        /// <param name="apiKey">The API key for accessing the Covalent API service.</param>
        /// <param name="chainId">The blockchain chain ID.</param>
        /// <param name="contractAddress">The Ethereum address of the ERC20 token contract.</param>
        /// <returns>An instance of the <see cref="GetCacheRequest"/> class initialized with the Covalent API service.</returns>
        /// <remarks>
        /// This method provides a convenient way to create a <see cref="GetCacheRequest"/> that interacts with the Covalent API service 
        /// for retrieving ERC20 token data. It simplifies the creation of <see cref="GetCacheRequest"/> by encapsulating 
        /// the creation of a <see cref="CovalentService"/> instance, ensuring that the correct service is used for interacting 
        /// with the Covalent API.
        /// </remarks>
        public static GetCacheRequest CreateWithCovalentService(string apiKey, long chainId, EthereumAddress contractAddress)
            => new GetCacheRequest(chainId, new CovalentService(apiKey, chainId, contractAddress));
    }
}