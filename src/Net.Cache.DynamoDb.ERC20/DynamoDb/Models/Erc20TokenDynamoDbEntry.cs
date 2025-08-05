using Net.Cryptography.SHA256;
using Net.Web3.EthereumWallet;
using Amazon.DynamoDBv2.DataModel;
using Net.Cache.DynamoDb.ERC20.RPC.Models;
using System;

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

        public Erc20TokenDynamoDbEntry(long chainId, EthereumAddress address, Erc20Token erc20Token)
        {
            HashKey = GenerateHashKey(chainId, address);
            Name = erc20Token.Name;
            Symbol = erc20Token.Symbol;
            Decimals = erc20Token.Decimals;
            TotalSupply = Nethereum.Web3.Web3.Convert.FromWei(erc20Token.TotalSupply, erc20Token.Decimals);
        }

        public static string GenerateHashKey(long chainId, EthereumAddress address)
        {
            return $"{chainId}-{address}".ToSha256();
        }
    }
}
