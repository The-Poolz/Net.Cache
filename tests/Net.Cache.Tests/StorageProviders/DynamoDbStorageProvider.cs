using Moq;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace Net.Cache.Tests.StorageProviders;

public class DynamoDbStorageProvider : IStorageProvider<string, string>
{
    private const string TableName = "table";
    protected readonly IAmazonDynamoDB client;

    public DynamoDbStorageProvider(IMock<IAmazonDynamoDB>? mock = null)
    {
        client = mock != null ? mock.Object : new Mock<IAmazonDynamoDB>().Object;
    }

    public void Store(string name, string description)
    {
        var item = new Dictionary<string, AttributeValue>
        {
            { "Name", new AttributeValue { S = name } },
            { "Description", new AttributeValue { S = description } }
        };

        var request = new PutItemRequest
        {
            TableName = TableName,
            Item = item
        };

        client.PutItemAsync(request)
            .GetAwaiter()
            .GetResult();
    }

    public bool TryGetValue(string name, out string description)
    {
        var request = new GetItemRequest
        {
            TableName = TableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { "Name", new AttributeValue { S = name } }
            }
        };

        var response = client.GetItemAsync(request)
            .GetAwaiter()
            .GetResult();

        if (response.Item == null || !response.Item.ContainsKey("Description"))
        {
            description = "N/A";
            return false;
        }

        description = response.Item["Description"].S;
        return true;
    }
}