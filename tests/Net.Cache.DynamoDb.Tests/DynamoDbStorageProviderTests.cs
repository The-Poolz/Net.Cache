using Moq;
using Xunit;
using FluentAssertions;
using Amazon.DynamoDBv2.DataModel;

namespace Net.Cache.DynamoDb.Tests;

public class DynamoDbStorageProviderTests
{
    private readonly Mock<IDynamoDBContext> mockContext;

    public DynamoDbStorageProviderTests()
    {
        mockContext = new Mock<IDynamoDBContext>();
    }

    [Fact]
    public void Constructor_WithoutParameters()
    {
        Environment.SetEnvironmentVariable("AWS_REGION", "eu-central-1");

        var provider = new DynamoDbStorageProvider<string, object>();

        provider.Should().NotBeNull();
    }

    [Fact]
    public void Store_ShouldCallSaveAsyncWithCorrectParameters()
    {
        var provider = new DynamoDbStorageProvider<string, object>(mockContext.Object);
        var value = new object();
        const string key = "testKey";

        provider.Store(key, value);

        mockContext.Verify(c => c.SaveAsync(value, default), Times.Once);
    }

    [Fact]
    public void TryGetValue_WhenValueExists_ShouldReturnTrueAndSetValue()
    {
        var expectedValue = new object();
        const string key = "testKey";
        mockContext.Setup(c => c.LoadAsync<object>(key, null, default)).ReturnsAsync(expectedValue);
        var provider = new DynamoDbStorageProvider<string, object>(mockContext.Object);

        var result = provider.TryGetValue(key, out var value);

        result.Should().BeTrue();
        value.Should().Be(expectedValue);
    }

    [Fact]
    public void TryGetValue_WhenValueDoesNotExist_ShouldReturnFalse()
    {
        const string key = "testKey";
        mockContext.Setup(c => c.LoadAsync<object>(key, null, default)).ReturnsAsync((object)null!);
        var provider = new DynamoDbStorageProvider<string, object>(mockContext.Object);

        var result = provider.TryGetValue(key, out var value);

        result.Should().BeFalse();
        value.Should().BeNull();
    }

    [Fact]
    public void TryGetValue_WhenExceptionOccurs_ShouldReturnFalse()
    {
        const string key = "testKey";
        mockContext.Setup(c => c.LoadAsync<object>(key, null, default)).ThrowsAsync(new Exception());
        var provider = new DynamoDbStorageProvider<string, object>(mockContext.Object);

        var result = provider.TryGetValue(key, out var value);

        result.Should().BeFalse();
        value.Should().BeNull();
    }
}