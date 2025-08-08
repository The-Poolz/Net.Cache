using Nethereum.Web3;
using Net.Web3.EthereumWallet;

namespace Net.Cache.DynamoDb.ERC20.Rpc
{
    /// <summary>
    /// Factory abstraction for creating instances of <see cref="IErc20Service"/>.
    /// </summary>
    public interface IErc20ServiceFactory
    {
        /// <summary>
        /// Creates an ERC20 service for the specified web3 client and multicall address.
        /// </summary>
        /// <param name="web3">The web3 client used to perform RPC calls.</param>
        /// <param name="multiCall">The address of the multicall contract.</param>
        /// <returns>A new <see cref="IErc20Service"/> instance.</returns>
        public IErc20Service Create(IWeb3 web3, EthereumAddress multiCall);
    }
}
