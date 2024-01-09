using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Net.Cache.DynamoDb.Models;

namespace Net.Cache.DynamoDb;

/// <summary>
/// An abstract base class for DynamoDB storage providers used in the Net.Cache library.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
public abstract class AbstractDynamoDbStorageProvider<TKey, TValue> : IStorageProvider<TKey, TValue>
    where TKey : IEquatable<TKey>
    where TValue : DynamoDbTable<TKey>
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

    public virtual void Store(TKey key, TValue value)
    {
        var item = new Dictionary<string, AttributeValue>();

        // TODO: Need to use custom attributes 
        //var typeHandlers = new Dictionary<Type, Func<object, AttributeValue>>
        //{
        //    { typeof(string), x => new AttributeValue { S = x.ToString() } },
        //    { typeof(bool), x => new AttributeValue { BOOL = (bool)x } },
        //    { typeof(sbyte), x => new AttributeValue { N = x.ToString() } },
        //    { typeof(byte), x => new AttributeValue { N = x.ToString() } },
        //    { typeof(short), x => new AttributeValue { N = x.ToString() } },
        //    { typeof(ushort), x => new AttributeValue { N = x.ToString() } },
        //    { typeof(int), x => new AttributeValue { N = x.ToString() } },
        //    { typeof(uint), x => new AttributeValue { N = x.ToString() } },
        //    { typeof(long), x => new AttributeValue { N = x.ToString() } },
        //    { typeof(ulong), x => new AttributeValue { N = x.ToString() } },
        //    { typeof(IEnumerable<string>), x => new AttributeValue { SS = (List<string>)x } },
        //    { typeof(string[]), x => new AttributeValue { SS = (List<string>)x } },
        //    { typeof(List<string>), x => new AttributeValue { SS = (List<string>)x } },
        //    { typeof(List<sbyte>), x => new AttributeValue { NS = (List<string>)x } },
        //    { typeof(List<byte>), x => new AttributeValue { NS = (List<string>)x } },
        //    { typeof(List<short>), x => new AttributeValue { NS = (List<string>)x } },
        //    { typeof(List<ushort>), x => new AttributeValue { NS = (List<string>)x } },
        //    { typeof(List<int>), x => new AttributeValue { NS = (List<string>)x } },
        //    { typeof(List<uint>), x => new AttributeValue { NS = (List<string>)x } },
        //    { typeof(List<long>), x => new AttributeValue { NS = (List<string>)x } },
        //    { typeof(List<ulong>), x => new AttributeValue { NS = (List<string>)x } },
        //};

        foreach (var prop in typeof(TValue).GetProperties())
        {
            var propValue = prop.GetValue(value);
            if (propValue == null) continue;

            if (typeHandlers.TryGetValue(prop.PropertyType, out var handler))
            {
                item[prop.Name] = handler(propValue);
            }
            else
            {
                throw new NotSupportedException($"'{prop.PropertyType}' type is not supported.");
            }
        }

        PutItem(item);
    }

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