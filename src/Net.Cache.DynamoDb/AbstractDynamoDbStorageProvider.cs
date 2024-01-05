using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace Net.Cache.DynamoDb;

/// <summary>
/// An abstract base class for DynamoDB storage providers used in the Net.Cache library.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
public abstract class AbstractDynamoDbStorageProvider<TKey, TValue> : IStorageProvider<TKey, TValue>
    where TKey : notnull
    where TValue : notnull
{
    private readonly string tableName;
    protected readonly IAmazonDynamoDB client;

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractDynamoDbStorageProvider{TKey, TValue}"/> class with the specified table name.
    /// </summary>
    /// <param name="tableName">The name of the DynamoDB table to use.</param>
    protected AbstractDynamoDbStorageProvider(string tableName)
        : this(tableName, new AmazonDynamoDBClient())
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractDynamoDbStorageProvider{TKey, TValue}"/> class with the specified table name and DynamoDB client.
    /// </summary>
    /// <param name="tableName">The name of the DynamoDB table to use.</param>
    /// <param name="client">The DynamoDB client to use for database operations.</param>
    protected AbstractDynamoDbStorageProvider(string tableName, IAmazonDynamoDB client)
    {
        this.tableName = tableName;
        this.client = client;
    }

    public abstract void Store(TKey key, TValue value);

    public abstract bool TryGetValue(TKey key, out TValue value);

    /// <summary>
    /// Gets an item from DynamoDB with the specified key.
    /// </summary>
    /// <param name="key">The key used to retrieve the item.</param>
    /// <returns>The response containing the item retrieved from DynamoDB.</returns>
    protected GetItemResponse GetItem(Dictionary<string, AttributeValue> key)
    {
        return client.GetItemAsync(new GetItemRequest 
        {
            TableName = tableName,
            Key = key
        }).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Puts an item into DynamoDB with the specified attributes.
    /// </summary>
    /// <param name="item">The item to put into DynamoDB.</param>
    /// <returns>The response indicating the result of the put operation.</returns>
    protected PutItemResponse PutItem(Dictionary<string, AttributeValue> item)
    {
        return client.PutItemAsync(new PutItemRequest
        {
            TableName = tableName,
            Item = item
        }).GetAwaiter().GetResult();
    }
}