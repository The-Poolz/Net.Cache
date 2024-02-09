using Amazon.DynamoDBv2.DataModel;
using Net.Cache.DynamoDb.ERC20.RPC;
using Net.Cache.DynamoDb.ERC20.Models;
using System.Diagnostics.CodeAnalysis;

namespace Net.Cache.DynamoDb.ERC20;

/// <summary>
/// Manages the storage and retrieval of ERC20 token information in a DynamoDB table.
/// </summary>
/// <remarks>
/// This provider extends the <see cref="DynamoDbStorageProvider{Tkey, TValue}"/> to specifically handle the operations
/// related to storing and accessing ERC20 token data. It includes functionalities to update
/// the total supply of a token and to retrieve or store token information based on a given key.
/// </remarks>
public class ERC20StorageProvider : DynamoDbStorageProvider<string, ERC20DynamoDbTable>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ERC20StorageProvider"/> class with an optional <paramref name="tableName"/>.
    /// </summary>
    /// <param name="tableName">The name of the DynamoDB table to be used. If not provided, a default table name is used.</param>
    public ERC20StorageProvider(string tableName = EmptyString)
        : base(tableName)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ERC20StorageProvider"/> class with a specific DynamoDB <paramref name="context"/> and an optional <paramref name="tableName"/>.
    /// </summary>
    /// <param name="context">The DynamoDB context to be used for operations.</param>
    /// <param name="tableName">The name of the DynamoDB table to be used. If not provided, a default table name is used.</param>
    public ERC20StorageProvider(IDynamoDBContext context, string tableName = EmptyString)
        : base(context, tableName)
    { }

    /// <summary>
    /// Updates the total supply of an existing ERC20 token information entry.
    /// </summary>
    /// <param name="existValue">The existing entry of ERC20 token information.</param>
    /// <param name="erc20Service">The ERC20 service to use for updating the total supply.</param>
    /// <returns>The updated ERC20 token information entry.</returns>
    protected virtual ERC20DynamoDbTable UpdateTotalSupply(ERC20DynamoDbTable existValue, IERC20Service erc20Service)
    {
        var updatedValue = new ERC20DynamoDbTable(
            existValue.ChainId,
            existValue.Address,
            existValue.Name,
            existValue.Symbol,
            existValue.Decimals,
            erc20Service.TotalSupply()
        );
        Context.SaveAsync(updatedValue)
            .GetAwaiter()
            .GetResult();
        return updatedValue;
    }

    /// <summary>
    /// Tries to retrieve an ERC20 token information entry from the cache based on a given key.
    /// </summary>
    /// <param name="key">The key associated with the ERC20 token information.</param>
    /// <param name="request">The request containing details for the retrieval operation.</param>
    /// <param name="value">When this method returns, contains the ERC20 token information associated with the specified key, if the key is found; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the token information is found; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// This method also updates the total supply of the token information if the <see cref="GetCacheRequest.UpdateTotalSupply"/> flag is set in the <paramref name="request"/>.
    /// </remarks>
    public virtual bool TryGetValue(string key, GetCacheRequest request, [MaybeNullWhen(false)] out ERC20DynamoDbTable value)
    {
        if (!base.TryGetValue(key, out value))
        {
            return false;
        }

        if (request.UpdateTotalSupply)
        {
            value = UpdateTotalSupply(value, request.ERC20Service);
        }

        return true;
    }
}