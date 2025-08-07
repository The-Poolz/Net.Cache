using Nethereum.Web3;
using Net.Web3.EthereumWallet;

namespace Net.Cache.DynamoDb.ERC20.Rpc
{
    public interface IErc20ServiceFactory
    {
        public IErc20Service Create(IWeb3 web3, EthereumAddress multiCall);
    }
}
