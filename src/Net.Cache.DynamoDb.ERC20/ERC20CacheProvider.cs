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

    public ERC20CacheProvider(DynamoDbStorageProvider<string, ERC20DynamoDbTable> storageProvider)
        : base(storageProvider)
    { }

    public virtual ERC20DynamoDbTable GetOrAdd(string key, BigInteger chainId, IERC20Service erc20Service)
    {
        var decimals = erc20Service.Decimals();
        return GetOrAdd(key, _ => new ERC20DynamoDbTable(
            chainId,
            erc20Service.ContractAddress,
            erc20Service.Name(),
            erc20Service.Symbol(),
            decimals,
            Nethereum.Web3.Web3.Convert.FromWei(erc20Service.TotalSupply(), decimals)
        ));
    }

    public virtual ERC20DynamoDbTable GetOrAdd(string key, BigInteger chainId, EthereumAddress contractAddress, string rpcUrl) =>
        GetOrAdd(key, chainId, new ERC20Service(rpcUrl, contractAddress));
}