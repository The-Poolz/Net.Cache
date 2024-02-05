using System.Numerics;
using Net.Web3.EthereumWallet;
using Net.Cryptography.SHA256;
using Amazon.DynamoDBv2.DataModel;
using Net.Cache.DynamoDb.ERC20.RPC;

namespace Net.Cache.DynamoDb.ERC20.Models;

[DynamoDBTable("TokensInfoCache")]
public class ERC20DynamoDbTable
{
    [DynamoDBHashKey]
    public string HashKey { get; }

    [DynamoDBProperty]
    public BigInteger ChainId { get; }

    [DynamoDBProperty]
    public string Address { get; }

    [DynamoDBProperty]
    public string Name { get; }

    [DynamoDBProperty]
    public string Symbol { get; }

    [DynamoDBProperty]
    public byte Decimals { get; }

    [DynamoDBProperty]
    public decimal TotalSupply { get; }

    public ERC20DynamoDbTable(BigInteger chainId, EthereumAddress address, string name, string symbol, byte decimals, decimal totalSupply)
    {
        ChainId = chainId;
        Address = address;
        Name = name;
        Symbol = symbol;
        Decimals = decimals;
        TotalSupply = totalSupply;
        HashKey = $"{ChainId}-{Address}".ToSha256();
    }

    public ERC20DynamoDbTable(BigInteger chainId, EthereumAddress address, string name, string symbol, byte decimals, BigInteger totalSupply)
    {
        ChainId = chainId;
        Address = address;
        Name = name;
        Symbol = symbol;
        Decimals = decimals;
        TotalSupply = Nethereum.Web3.Web3.Convert.FromWei(totalSupply, decimals);
        HashKey = $"{ChainId}-{Address}".ToSha256();
    }

    public ERC20DynamoDbTable(BigInteger chainId, IERC20Service erc20Service)
    {
        ChainId = chainId;
        Address = erc20Service.ContractAddress;
        Name = erc20Service.Name();
        Symbol = erc20Service.Symbol();
        Decimals = erc20Service.Decimals();
        TotalSupply = Nethereum.Web3.Web3.Convert.FromWei(erc20Service.TotalSupply(), Decimals);
        HashKey = $"{ChainId}-{Address}".ToSha256();
    }
}