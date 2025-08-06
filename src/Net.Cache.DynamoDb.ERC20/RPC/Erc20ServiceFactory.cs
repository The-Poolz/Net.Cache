using Nethereum.Web3;
using Net.Web3.EthereumWallet;

namespace Net.Cache.DynamoDb.ERC20.RPC
{
    public class Erc20ServiceFactory : IErc20ServiceFactory
    {
        public IErc20Service Create(IWeb3 web3, EthereumAddress multiCall)
        {
            return new Erc20Service(web3, multiCall);
        }
    }
}
