using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace Net.Cache.DynamoDb;

/// <summary>
/// Class for DynamoDB storage providers used in the Net.Cache library.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
public class DynamoDbStorageProvider<TKey, TValue> : IStorageProvider<TKey, TValue>
    where TKey : IEquatable<TKey>
    where TValue : class
{
    protected readonly Lazy<IDynamoDBContext> lazyContext;
    protected IDynamoDBContext Context => lazyContext.Value;

    /// <summary>
    /// Initializes a new instance of the <see cref="DynamoDbStorageProvider{TKey,TValue}"/> class with the specified table name.
    /// </summary>
    public DynamoDbStorageProvider()
        : this(new AmazonDynamoDBClient())
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DynamoDbStorageProvider{TKey,TValue}"/> class with the specified DynamoDB client.
    /// </summary>
    /// <param name="client">The DynamoDB client to use for database operations.</param>
    public DynamoDbStorageProvider(IAmazonDynamoDB client)
    {
        lazyContext = new Lazy<IDynamoDBContext>(new DynamoDBContext(client));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DynamoDbStorageProvider{TKey,TValue}"/> class with the specified DynamoDB context.
    /// </summary>
    /// <param name="context">The DynamoDB context to use for database operations.</param>
    public DynamoDbStorageProvider(IDynamoDBContext context)
    {
        lazyContext = new Lazy<IDynamoDBContext>(context);
    }

    public void Store(TKey key, TValue value)
    {
        Context.SaveAsync(value);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        value = default;
        try
        {
            value = Context.LoadAsync<TValue>(key)
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