using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace Net.Cache.DynamoDb;

public abstract class AbstractDynamoDbStorageProvider<TKey, TValue> : IStorageProvider<TKey, TValue>
{
    private readonly string tableName;
    protected readonly IAmazonDynamoDB client;

    protected AbstractDynamoDbStorageProvider(string tableName)
        : this(tableName, new AmazonDynamoDBClient())
    { }

    protected AbstractDynamoDbStorageProvider(string tableName, IAmazonDynamoDB client)
    {
        this.tableName = tableName;
        this.client = client;
    }

    public abstract void Store(TKey key, TValue value);
    public abstract bool TryGetValue(TKey key, out TValue value);

    protected GetItemResponse GetItem(Dictionary<string, AttributeValue> key)
    {
        return client.GetItemAsync(new GetItemRequest 
        {
            TableName = tableName,
            Key = key
        }).GetAwaiter().GetResult();
    }

    protected PutItemResponse PutItem(Dictionary<string, AttributeValue> item)
    {
        return client.PutItemAsync(new PutItemRequest
        {
            TableName = tableName,
            Item = item
        }).GetAwaiter().GetResult();
    }
}