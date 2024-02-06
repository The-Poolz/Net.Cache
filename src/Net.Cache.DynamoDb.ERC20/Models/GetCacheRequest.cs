using System.Numerics;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20.RPC;

namespace Net.Cache.DynamoDb.ERC20.Models;

public class GetCacheRequest
{
    public BigInteger ChainId { get; }
    public IERC20Service ERC20Service { get; }
    public bool UpdateTotalSupply { get; }

    public GetCacheRequest(BigInteger chainId, EthereumAddress contractAddress, string rpcUrl, bool updateTotalSupply = true)
        : this(chainId, new ERC20Service(rpcUrl, contractAddress), updateTotalSupply)
    { }

    public GetCacheRequest(BigInteger chainId, IERC20Service erc20Service, bool updateTotalSupply = true)
    {
        ChainId = chainId;
        ERC20Service = erc20Service;
        UpdateTotalSupply = updateTotalSupply;
    }
}