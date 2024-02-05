using Amazon.DynamoDBv2.DataModel;
using Net.Cache.DynamoDb.ERC20.RPC;
using Net.Cache.DynamoDb.ERC20.Models;
using System.Diagnostics.CodeAnalysis;

namespace Net.Cache.DynamoDb.ERC20;

public class ERC20StorageProvider : DynamoDbStorageProvider<string, ERC20DynamoDbTable>
{
    public ERC20StorageProvider(string? tableName = EmptyString)
        : base(tableName)
    { }

    public ERC20StorageProvider(IDynamoDBContext context, string? tableName = EmptyString)
        : base(context, tableName)
    { }

    protected ERC20DynamoDbTable UpdateTotalSupply(ERC20DynamoDbTable existValue, IERC20Service erc20Service)
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

    public bool TryGetValue(string key, GetCacheRequest request, [MaybeNullWhen(false)] out ERC20DynamoDbTable value)
    {
        value = default;
        try
        {
            var operationConfig = string.IsNullOrWhiteSpace(tableName) ? null : new DynamoDBOperationConfig
            {
                OverrideTableName = tableName
            };
            value = Context.LoadAsync<ERC20DynamoDbTable>(key, operationConfig)
                .GetAwaiter()
                .GetResult();

            if (value == null)
            {
                return false;
            }
            if (request.UpdateTotalSupply)
            {
                value = UpdateTotalSupply(value, request.ERC20Service);
            }
            return true;
        }
        catch
        {
            return false;
        }
    }
}