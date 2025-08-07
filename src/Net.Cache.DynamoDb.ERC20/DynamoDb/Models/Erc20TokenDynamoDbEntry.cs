using Amazon.DynamoDBv2.DataModel;
using Net.Cache.DynamoDb.ERC20.Rpc.Models;

namespace Net.Cache.DynamoDb.ERC20.DynamoDb.Models
{
    [DynamoDBTable("TokensInfoCache")]
    public class Erc20TokenDynamoDbEntry
    {
        [DynamoDBHashKey]
        public string HashKey { get; set; } = string.Empty;

        [DynamoDBProperty]
        public long ChainId { get; set; }

        [DynamoDBProperty]
        public string Address { get; set; } = string.Empty;

        [DynamoDBProperty]
        public string Name { get; set; } = string.Empty;

        [DynamoDBProperty]
        public string Symbol { get; set; } = string.Empty;

        [DynamoDBProperty]
        public byte Decimals { get; set; }

        [DynamoDBProperty]
        public decimal TotalSupply { get; set; }

        /// <summary>
        /// Constructor without parameters for working "AWSSDK.DynamoDBv2"
        /// </summary>
        public Erc20TokenDynamoDbEntry() { }

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
