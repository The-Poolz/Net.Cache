using System.Numerics;
using Net.Web3.EthereumWallet;

namespace Net.Cache.DynamoDb.ERC20.RPC;

/// <summary>
/// Defines the basic functionalities for interacting with an ERC20 token contract.
/// </summary>
/// <remarks>
/// This interface provides methods for accessing key properties of an ERC20 token,
/// such as its contract address, decimals, name, symbol, and total supply. Implementations
/// of this interface should encapsulate the logic necessary to query these properties from the block-chain.
/// </remarks>
public interface IERC20Service
{
    /// <summary>
    /// Gets the contract address of the ERC20 token.
    /// </summary>
    /// <value>The Ethereum address of the ERC20 token contract.</value>
    public EthereumAddress ContractAddress { get; }

    /// <summary>
    /// Retrieves the number of decimals the ERC20 token uses.
    /// </summary>
    /// <returns>The number of decimals for the ERC20 token.</returns>
    public byte Decimals();

    /// <summary>
    /// Retrieves the name of the ERC20 token.
    /// </summary>
    /// <returns>The name of the ERC20 token.</returns>
    public string Name();

    /// <summary>
    /// Retrieves the symbol of the ERC20 token.
    /// </summary>
    /// <returns>The symbol of the ERC20 token.</returns>
    public string Symbol();

    /// <summary>
    /// Retrieves the total supply of the ERC20 token.
    /// </summary>
    /// <returns>The total supply of the ERC20 token as a BigInteger.</returns>
    public BigInteger TotalSupply();
}