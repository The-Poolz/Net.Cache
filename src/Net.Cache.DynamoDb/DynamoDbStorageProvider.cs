using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace Net.Cache.DynamoDb;

/// <summary>
/// An abstract base class for DynamoDB storage providers used in the Net.Cache library.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
public class DynamoDbStorageProvider<TKey, TValue> : IStorageProvider<TKey, TValue>
    where TKey : IEquatable<TKey>
    where TValue : notnull
{
    protected readonly IDynamoDBContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="DynamoDbStorageProvider{TKey,TValue}"/> class with the specified table name.
    /// </summary>
    protected DynamoDbStorageProvider()
        : this(new AmazonDynamoDBClient())
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DynamoDbStorageProvider{TKey,TValue}"/> class with the specified DynamoDB client.
    /// </summary>
    /// <param name="client">The DynamoDB client to use for database operations.</param>
    protected DynamoDbStorageProvider(IAmazonDynamoDB client)
    {
        context = new DynamoDBContext(client);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DynamoDbStorageProvider{TKey,TValue}"/> class with the specified DynamoDB context.
    /// </summary>
    /// <param name="context">The DynamoDB context to use for database operations.</param>
    protected DynamoDbStorageProvider(IDynamoDBContext context)
    {
        this.context = context;
    }

    public void Store(TKey key, TValue value)
    {
        context.SaveAsync(value);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        value = default;
        try
        {
            value = context.LoadAsync<TValue>(key)
                .GetAwaiter()
                .GetResult();
            return value != null;
        }
        catch
        {
            return false;
        }
    }
}