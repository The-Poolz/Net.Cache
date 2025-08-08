using Nethereum.Web3;
using Net.Web3.EthereumWallet;

namespace Net.Cache.DynamoDb.ERC20.Rpc
{
    /// <summary>
    /// Default implementation of <see cref="IErc20ServiceFactory"/>.
    /// </summary>
    public class Erc20ServiceFactory : IErc20ServiceFactory
    {
        /// <inheritdoc cref="IErc20ServiceFactory.Create"/>
        public IErc20Service Create(IWeb3 web3, EthereumAddress multiCall)
        {
            return new Erc20Service(web3, multiCall);
        }
    }
}
