using System.Numerics;
using Net.Web3.EthereumWallet;

namespace Net.Cache.DynamoDb.ERC20.RPC;

public interface IERC20Service
{
    public EthereumAddress ContractAddress { get; }
    public byte Decimals();
    public string Name();
    public string Symbol();
    public BigInteger TotalSupply();
}