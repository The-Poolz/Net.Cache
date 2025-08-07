using System.Threading.Tasks;
using Net.Cache.DynamoDb.ERC20.Rpc.Models;
using Net.Web3.EthereumWallet;

namespace Net.Cache.DynamoDb.ERC20.Rpc
{
    public interface IErc20Service
    {
        public Erc20TokenData GetErc20Token(EthereumAddress token);
        public Task<Erc20TokenData> GetErc20TokenAsync(EthereumAddress token);
    }
}
