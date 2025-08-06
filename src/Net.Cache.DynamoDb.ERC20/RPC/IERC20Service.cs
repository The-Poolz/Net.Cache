using System.Threading.Tasks;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20.RPC.Models;

namespace Net.Cache.DynamoDb.ERC20.RPC
{
    public interface IErc20Service
    {
        public Erc20TokenData GetEr20Token(EthereumAddress token);
        public Task<Erc20TokenData> GetErc20TokenAsync(EthereumAddress token);
    }
}
