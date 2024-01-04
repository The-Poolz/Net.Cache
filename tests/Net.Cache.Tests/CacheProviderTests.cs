using Moq;
using Xunit;
using FluentAssertions;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Net.Cache.Tests.StorageProviders;

namespace Net.Cache.Tests;

public class CacheProviderTests
{
    private const string name = "Cat";
    private const string description = "Description";

    [Fact]
    public void GetOrCache_ShouldReturnCachedValue_IfKeyExistsInCache()
    {
        var storageProvider = new DynamoDbStorageProvider();
        var cacheProvider = new CacheProvider<string, string>(storageProvider);
        cacheProvider.TryAdd(name, description);

        var result = cacheProvider.GetOrAdd(name, () => "New Description");

        result.Should().Be(description);
    }

    [Fact]
    public void GetOrCache_ShouldAddValueToCache_IfKeyNotExists()
    {
        var clientMock = new Mock<IAmazonDynamoDB>();
        clientMock
            .Setup(x => x.GetItemAsync(It.IsAny<GetItemRequest>(), default))
            .ReturnsAsync(new GetItemResponse());
        var storageProvider = new DynamoDbStorageProvider(clientMock);
        const string newDescription = "New Description";
        var cacheProvider = new CacheProvider<string, string>(storageProvider);

        var result = cacheProvider.GetOrAdd(name, () => newDescription);

        result.Should().Be(newDescription);
        clientMock.Verify(x => x.PutItemAsync(It.IsAny<PutItemRequest>(), default), Times.Once);
    }

    [Fact]
    public void GetOrCache_ShouldGetValueFromStorageProvider_IfKeyNotExistsInCache()
    {
        var clientMock = new Mock<IAmazonDynamoDB>();
        clientMock
            .Setup(x => x.GetItemAsync(It.IsAny<GetItemRequest>(), default))
            .ReturnsAsync(new GetItemResponse
            {
                Item = new Dictionary<string, AttributeValue>
                {
                    { "Name", new AttributeValue(name) },
                    { "Description", new AttributeValue(description) }
                }
            });
        var storageProvider = new DynamoDbStorageProvider(clientMock);
        var cacheProvider = new CacheProvider<string, string>(storageProvider);

        var result = cacheProvider.GetOrAdd(name, _ => "New Description");

        result.Should().Be(description);
    }
}