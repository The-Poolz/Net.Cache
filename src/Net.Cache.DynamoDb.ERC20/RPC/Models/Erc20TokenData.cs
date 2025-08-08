using System.Numerics;
using Net.Web3.EthereumWallet;

namespace Net.Cache.DynamoDb.ERC20.Rpc.Models
{
    /// <summary>
    /// Represents ERC20 token metadata retrieved from the blockchain.
    /// </summary>
    public class Erc20TokenData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Erc20TokenData"/> class.
        /// </summary>
        /// <param name="address">The token contract address.</param>
        /// <param name="name">The token name.</param>
        /// <param name="symbol">The token symbol.</param>
        /// <param name="decimals">The number of decimal places used by the token.</param>
        /// <param name="totalSupply">The total token supply.</param>
        public Erc20TokenData(EthereumAddress address, string name, string symbol, byte decimals, BigInteger totalSupply)
        {
            Address = address;
            Name = name;
            Symbol = symbol;
            Decimals = decimals;
            TotalSupply = totalSupply;
        }

        /// <summary>
        /// Gets the token contract address.
        /// </summary>
        public EthereumAddress Address { get; }

        /// <summary>
        /// Gets the token name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the token symbol.
        /// </summary>
        public string Symbol { get; }

        /// <summary>
        /// Gets the number of decimal places used by the token.
        /// </summary>
        public byte Decimals { get; }

        /// <summary>
        /// Gets the total token supply.
        /// </summary>
        public BigInteger TotalSupply { get; }
    }
}
