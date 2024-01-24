using Xunit;
using FluentAssertions;

namespace Net.Cache.DynamoDb.ERC20.Tests;

public class ERC20CacheProviderTests
{
    [Fact]
    internal void Test()
    {
        const string message = "hello world!";

        message.Should().Be(message);
    }
}