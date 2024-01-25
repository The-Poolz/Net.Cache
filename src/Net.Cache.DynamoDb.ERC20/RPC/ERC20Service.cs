using Nethereum.Web3;
using System.Numerics;
using Net.Web3.EthereumWallet;
using Nethereum.Contracts.Standards.ERC20;

namespace Net.Cache.DynamoDb.ERC20.RPC;

public class ERC20Service : IERC20Service
{
    private readonly ERC20ContractService contractService;
    public EthereumAddress ContractAddress { get; }

    public ERC20Service(string rpcUrl, EthereumAddress contractAddress)
        : this(new Nethereum.Web3.Web3(rpcUrl), contractAddress)
    { }

    public ERC20Service(IWeb3 web3, EthereumAddress contractAddress)
    {
        ContractAddress = contractAddress;
        contractService = web3.Eth.ERC20.GetContractService(contractAddress);
    }

    public virtual byte Decimals() => contractService.DecimalsQueryAsync().GetAwaiter().GetResult();

    public virtual string Name() => contractService.NameQueryAsync().GetAwaiter().GetResult();

    public virtual string Symbol() => contractService.SymbolQueryAsync().GetAwaiter().GetResult();

    public virtual BigInteger TotalSupply() => contractService.TotalSupplyQueryAsync().GetAwaiter().GetResult();
}