//using System.Numerics;
//using Net.Web3.EthereumWallet;
//using Net.Cryptography.SHA256;
//using Amazon.DynamoDBv2.DataModel;
//using Net.Cache.DynamoDb.ERC20.RPC;

//namespace Net.Cache.DynamoDb.ERC20.Models
//{
//    /// <summary>
//    /// Represents an ERC20 token information cache entry in DynamoDB.
//    /// </summary>
//    /// <remarks>
//    /// This class is designed to store and manage the basic details of an ERC20 token,
//    /// including its chain ID, address, name, symbol, decimals, and total supply. It also
//    /// generates a unique hash key for each token based on its chain ID and address.
//    /// </remarks>
//    [DynamoDBTable("TokensInfoCache")]
//    public class ERC20DynamoDbTable
//    {
//        /// <summary>
//        /// Gets the hash key for the ERC20 token entry, uniquely generated based on the chain ID and token address.
//        /// </summary>
//        [DynamoDBHashKey]
//        public string HashKey { get; set; } = string.Empty;

//        /// <summary>
//        /// Gets the block-chain chain ID where the ERC20 token is located.
//        /// </summary>
//        [DynamoDBProperty]
//        public long ChainId { get; set; }

//        /// <summary>
//        /// Gets the ERC20 token contract address.
//        /// </summary>
//        [DynamoDBProperty]
//        public string Address { get; set; } = string.Empty;

//        /// <summary>
//        /// Gets the name of the ERC20 token.
//        /// </summary>
//        [DynamoDBProperty]
//        public string Name { get; set; } = string.Empty;

//        /// <summary>
//        /// Gets the symbol of the ERC20 token.
//        /// </summary>
//        [DynamoDBProperty]
//        public string Symbol { get; set; } = string.Empty;

//        /// <summary>
//        /// Gets the decimals of the ERC20 token, indicating how divisible it is.
//        /// </summary>
//        [DynamoDBProperty]
//        public byte Decimals { get; set; }

//        /// <summary>
//        /// Gets the total supply of the ERC20 token.
//        /// </summary>
//        [DynamoDBProperty]
//        public decimal TotalSupply { get; set; }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="ERC20DynamoDbTable"/> class with specified token details.
//        /// </summary>
//        /// <param name="chainId">The block-chain chain ID.</param>
//        /// <param name="address">The ERC20 token contract address.</param>
//        /// <param name="name">The name of the ERC20 token.</param>
//        /// <param name="symbol">The symbol of the ERC20 token.</param>
//        /// <param name="decimals">The decimals of the ERC20 token.</param>
//        /// <param name="totalSupply">The total supply of the ERC20 token.</param>
//        /// <remarks>
//        /// This constructor expects an already calculated <paramref name="totalSupply"/> based on the <see cref="BigInteger"/> total supply value and <paramref name="decimals"/>.
//        /// </remarks>
//        public ERC20DynamoDbTable(long chainId, EthereumAddress address, string name, string symbol, byte decimals, decimal totalSupply)
//        {
//            ChainId = chainId;
//            Address = address;
//            Name = name;
//            Symbol = symbol;
//            Decimals = decimals;
//            TotalSupply = totalSupply;
//            HashKey = $"{ChainId}-{Address}".ToSha256();
//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="ERC20DynamoDbTable"/> class with specified token details and total supply in <see cref="BigInteger"/>.
//        /// </summary>
//        /// <param name="chainId">The block-chain chain ID.</param>
//        /// <param name="address">The ERC20 token contract address.</param>
//        /// <param name="name">The name of the ERC20 token.</param>
//        /// <param name="symbol">The symbol of the ERC20 token.</param>
//        /// <param name="decimals">The decimals of the ERC20 token.</param>
//        /// <param name="totalSupply">The total supply of the ERC20 token in <see cref="BigInteger"/> format.</param>
//        /// <remarks>
//        /// This constructor converts the <paramref name="totalSupply"/> from <see cref="BigInteger"/> to <see langword="decimal"/>, considering the token's <paramref name="decimals"/>.
//        /// </remarks>
//        public ERC20DynamoDbTable(long chainId, EthereumAddress address, string name, string symbol, byte decimals, BigInteger totalSupply)
//        {
//            ChainId = chainId;
//            Address = address;
//            Name = name;
//            Symbol = symbol;
//            Decimals = decimals;
//            TotalSupply = Nethereum.Web3.Web3.Convert.FromWei(totalSupply, decimals);
//            HashKey = $"{ChainId}-{Address}".ToSha256();
//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="ERC20DynamoDbTable"/> class using an <see cref="IERC20Service"/> to populate token details.
//        /// </summary>
//        /// <param name="chainId">The block-chain chain ID.</param>
//        /// <param name="erc20Service">The ERC20 service providing access to token details.</param>
//        /// <remarks>
//        /// This constructor retrieves token details such as name, symbol, decimals, and total supply from the provided <see cref="IERC20Service"/>.
//        /// </remarks>
//        public ERC20DynamoDbTable(long chainId, IERC20Service erc20Service)
//        {
//            ChainId = chainId;
//            Address = erc20Service.ContractAddress;
//            Name = erc20Service.Name();
//            Symbol = erc20Service.Symbol();
//            Decimals = erc20Service.Decimals();
//            TotalSupply = Nethereum.Web3.Web3.Convert.FromWei(erc20Service.TotalSupply(), Decimals);
//            HashKey = $"{ChainId}-{Address}".ToSha256();
//        }

//        /// <summary>
//        /// Constructor without parameters for working "Amazon.DynamoDBv2"
//        /// </summary>
//        public ERC20DynamoDbTable() { }
//    }
//}