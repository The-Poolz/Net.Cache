using System.Numerics;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20.RPC;
using Net.Cache.DynamoDb.ERC20.Models;

namespace Net.Cache.DynamoDb.ERC20;

public class ERC20CacheProvider : CacheProvider<string, ERC20DynamoDbTable>
{
    public ERC20CacheProvider()
        : base(new DynamoDbStorageProvider<string, ERC20DynamoDbTable>())
    { }

    public virtual ERC20DynamoDbTable GetOrAdd(string key, BigInteger chainId, ERC20Service erc20Service) =>
        GetOrAdd(key, _ => new ERC20DynamoDbTable(
            chainId,
            erc20Service.ContractAddress,
            erc20Service.Name(),
            erc20Service.Symbol(),
            erc20Service.Decimals(),
            erc20Service.TotalSupply()
        ));

    public virtual ERC20DynamoDbTable GetOrAdd(string key, BigInteger chainId, EthereumAddress contractAddress, string rpcUrl) =>
        GetOrAdd(key, chainId, new ERC20Service(rpcUrl, contractAddress));
}