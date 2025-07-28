using System;
using Net.Web3.EthereumWallet;

namespace Net.Cache.DynamoDb.ERC20.RPC.Exceptions
{
    public sealed class Erc20QueryException : Exception
    {
        public EthereumAddress Token { get; }

        public Erc20QueryException(EthereumAddress token, string message, Exception? inner = null)
            : base($"[ERC20 {token}] {message}", inner)
        {
            Token = token;
        }
    }
}
