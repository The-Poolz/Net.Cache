using System.Threading.Tasks;
using Net.Cache.DynamoDb.ERC20.Rpc.Models;
using Net.Web3.EthereumWallet;

namespace Net.Cache.DynamoDb.ERC20.Rpc
{
    /// <summary>
    /// Defines operations for retrieving ERC20 token information via RPC calls.
    /// </summary>
    public interface IErc20Service
    {
        /// <summary>
        /// Retrieves token information synchronously.
        /// </summary>
        /// <param name="token">The token contract address.</param>
        /// <returns>The token data.</returns>
        public Erc20TokenData GetErc20Token(EthereumAddress token);

        /// <summary>
        /// Retrieves token information asynchronously.
        /// </summary>
        /// <param name="token">The token contract address.</param>
        /// <returns>A task that resolves to the token data.</returns>
        public Task<Erc20TokenData> GetErc20TokenAsync(EthereumAddress token);
    }
}
