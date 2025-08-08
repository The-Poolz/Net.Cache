using Amazon.DynamoDBv2.DataModel;
using Net.Cache.DynamoDb.ERC20.Rpc.Models;

namespace Net.Cache.DynamoDb.ERC20.DynamoDb.Models
{
    /// <summary>
    /// Represents a persisted ERC20 token entry in the DynamoDB cache table.
    /// </summary>
    [DynamoDBTable("TokensInfoCache")]
    public class Erc20TokenDynamoDbEntry
    {
        /// <summary>
        /// Gets or sets the composite hash key value.
        /// </summary>
        [DynamoDBHashKey]
        public string HashKey { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the blockchain network identifier.
        /// </summary>
        [DynamoDBProperty]
        public long ChainId { get; set; }

        /// <summary>
        /// Gets or sets the ERC20 token contract address.
        /// </summary>
        [DynamoDBProperty]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the token name.
        /// </summary>
        [DynamoDBProperty]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the token symbol.
        /// </summary>
        [DynamoDBProperty]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the number of decimal places used by the token.
        /// </summary>
        [DynamoDBProperty]
        public byte Decimals { get; set; }

        /// <summary>
        /// Gets or sets the total supply of the token.
        /// </summary>
        [DynamoDBProperty]
        public decimal TotalSupply { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Erc20TokenDynamoDbEntry"/> class.<br/>
        /// Constructor without parameters for working "AWSSDK.DynamoDBv2" library.
        /// </summary>
        public Erc20TokenDynamoDbEntry() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Erc20TokenDynamoDbEntry"/> class with specified values.
        /// </summary>
        /// <param name="hashKey">The hash key identifying the token.</param>
        /// <param name="erc20Token">The token data retrieved from RPC.</param>
        public Erc20TokenDynamoDbEntry(HashKey hashKey, Erc20TokenData erc20Token)
        {
            HashKey = hashKey.Value;
            ChainId = hashKey.ChainId;
            Address = hashKey.Address;
            Name = erc20Token.Name;
            Symbol = erc20Token.Symbol;
            Decimals = erc20Token.Decimals;
            TotalSupply = Nethereum.Web3.Web3.Convert.FromWei(erc20Token.TotalSupply, erc20Token.Decimals);
        }
    }
}
