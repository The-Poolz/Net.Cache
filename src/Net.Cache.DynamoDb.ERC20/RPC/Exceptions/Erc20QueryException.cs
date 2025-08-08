using System;
using Net.Web3.EthereumWallet;

namespace Net.Cache.DynamoDb.ERC20.Rpc.Exceptions
{
    /// <summary>
    /// Represents errors that occur during ERC20 token queries.
    /// </summary>
    public sealed class Erc20QueryException : Exception
    {
        /// <summary>
        /// Gets the address of the token that caused the error.
        /// </summary>
        public EthereumAddress Token { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Erc20QueryException"/> class.
        /// </summary>
        /// <param name="token">The token contract address.</param>
        /// <param name="message">The error message.</param>
        /// <param name="inner">An optional inner exception.</param>
        public Erc20QueryException(EthereumAddress token, string message, Exception? inner = null)
            : base($"[ERC20 {token}] {message}", inner)
        {
            Token = token;
        }
    }
}
