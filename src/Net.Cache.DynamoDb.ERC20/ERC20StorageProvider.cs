﻿using Amazon.DynamoDBv2.DataModel;
using Net.Cache.DynamoDb.ERC20.RPC;
using Net.Cache.DynamoDb.ERC20.Models;
using System.Diagnostics.CodeAnalysis;

namespace Net.Cache.DynamoDb.ERC20;

public class ERC20StorageProvider : DynamoDbStorageProvider<string, ERC20DynamoDbTable>
{
    public ERC20StorageProvider() { }
    public ERC20StorageProvider(IDynamoDBContext context) : base(context) { }

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

    public bool TryGetValue(string key, GetCacheRequest request, [MaybeNullWhen(false)] out ERC20DynamoDbTable value)
    {
        value = default;
        try
        {
            value = Context.LoadAsync<ERC20DynamoDbTable>(key)
                .GetAwaiter()
                .GetResult();

            if (value == null)
            {
                return false;
            }
            if (request.UpdateTotalSupply)
            {
                UpdateTotalSupply(value, request.ERC20Service);
            }
            return true;
        }
        catch
        {
            return false;
        }
    }
}