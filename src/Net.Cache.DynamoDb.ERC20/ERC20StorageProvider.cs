using Net.Cache.DynamoDb.ERC20.RPC;
using Net.Cache.DynamoDb.ERC20.Models;
using System.Diagnostics.CodeAnalysis;

namespace Net.Cache.DynamoDb.ERC20;

public class ERC20StorageProvider : DynamoDbStorageProvider<string, ERC20DynamoDbTable>
{
    protected void UpdateTotalSupply(ERC20DynamoDbTable existValue, IERC20Service erc20Service)
    {
        Context.SaveAsync(new ERC20DynamoDbTable(
                existValue.ChainId,
                existValue.Address,
                existValue.Name,
                existValue.Symbol,
                existValue.Decimals,
                erc20Service.TotalSupply()
            ))
            .GetAwaiter()
            .GetResult();
    }

    public bool TryGetValue(string key, IERC20Service erc20Service, [MaybeNullWhen(false)] out ERC20DynamoDbTable value)
    {
        value = default;
        try
        {
            value = Context.LoadAsync<ERC20DynamoDbTable>(key)
                .GetAwaiter()
                .GetResult();

            if (value == null) return false;

            UpdateTotalSupply(value, erc20Service);
            return true;

        }
        catch
        {
            return false;
        }
    }
}