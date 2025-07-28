using System.Numerics;
using Net.Web3.EthereumWallet;

namespace Net.Cache.DynamoDb.ERC20.RPC.Models
{
    public class Erc20Token
    {
        public Erc20Token(EthereumAddress address, string name, string symbol, byte decimals, BigInteger totalSupply)
        {
            Address = address;
            Name = name;
            Symbol = symbol;
            Decimals = decimals;
            TotalSupply = totalSupply;
        }

        public EthereumAddress Address { get; }
        public string Name { get; }
        public string Symbol { get; }
        public byte Decimals { get; }
        public BigInteger TotalSupply { get; }
    }
}
