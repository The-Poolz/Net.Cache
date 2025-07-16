using System;
using Nethereum.Web3;
using System.Numerics;
using Net.Web3.EthereumWallet;
using Nethereum.Contracts.Standards.ERC20;

namespace Net.Cache.DynamoDb.ERC20.RPC
{
    /// <summary>
    /// Provides functionalities to interact with ERC20 tokens on the block-chain via RPC calls.
    /// </summary>
    /// <remarks>
    /// This class encapsulates the interactions with an ERC20 token contract,
    /// allowing for queries such as retrieving the token's name, symbol, decimals, and total supply.
    /// </remarks>
    public class ERC20Service : IERC20Service
    {
        private readonly Lazy<ERC20ContractService> contractService;

        /// <inheritdoc cref="IERC20Service.ContractAddress"/>
        public EthereumAddress ContractAddress { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ERC20Service"/> class using an RPC URL and contract address.
        /// </summary>
        /// <param name="rpcUrl">The URL of the RPC endpoint to interact with the block-chain.</param>
        /// <param name="contractAddress">The ERC20 token contract address.</param>
        /// <remarks>
        /// This constructor initializes the contract service for interacting with the ERC20 token.
        /// </remarks>
        public ERC20Service(string rpcUrl, EthereumAddress contractAddress)
            : this(() => rpcUrl, contractAddress)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ERC20Service"/> class using a function that provides an RPC URL and contract address.
        /// </summary>
        /// <param name="rpcUrlFactory">A function that returns the RPC URL to interact with the block-chain.</param>
        /// <param name="contractAddress">The ERC20 token contract address.</param>
        /// <remarks>
        /// The RPC URL is retrieved only when the service methods are invoked, allowing for lazy initialization.
        /// </remarks>
        public ERC20Service(Func<string> rpcUrlFactory, EthereumAddress contractAddress)
        {
            ContractAddress = contractAddress;
            contractService = new Lazy<ERC20ContractService>(() =>
            {
                var web3 = new Nethereum.Web3.Web3(rpcUrlFactory());
                return web3.Eth.ERC20.GetContractService(contractAddress);
            });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ERC20Service"/> class using a <see cref="Nethereum.Web3.IWeb3"/> instance and contract address.
        /// </summary>
        /// <param name="web3">The Web3 instance for interacting with the Ethereum network.</param>
        /// <param name="contractAddress">The ERC20 token contract address.</param>
        /// <remarks>
        /// This constructor provides more flexibility by allowing the use of an existing Web3 instance.
        /// </remarks>
        public ERC20Service(IWeb3 web3, EthereumAddress contractAddress)
        {
            ContractAddress = contractAddress;
            contractService = new Lazy<ERC20ContractService>(() => web3.Eth.ERC20.GetContractService(contractAddress));
        }

        /// <inheritdoc cref="IERC20Service.Decimals()"/>
        public virtual byte Decimals() => contractService.Value.DecimalsQueryAsync().GetAwaiter().GetResult();

        /// <inheritdoc cref="IERC20Service.Name()"/>
        public virtual string Name() => contractService.Value.NameQueryAsync().GetAwaiter().GetResult();

        /// <inheritdoc cref="IERC20Service.Symbol()"/>
        public virtual string Symbol() => contractService.Value.SymbolQueryAsync().GetAwaiter().GetResult();

        /// <inheritdoc cref="IERC20Service.TotalSupply()"/>
        public virtual BigInteger TotalSupply() => contractService.Value.TotalSupplyQueryAsync().GetAwaiter().GetResult();
    }
}
