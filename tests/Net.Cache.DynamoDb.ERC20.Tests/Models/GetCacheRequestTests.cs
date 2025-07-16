using Xunit;
using FluentAssertions;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20.Models;

namespace Net.Cache.DynamoDb.ERC20.Tests.Models;

public class GetCacheRequestTests
{
    [Fact]
    public void Constructor_WithRpcUrlFactory_ShouldNotInvokeFactory()
    {
        var called = false;

        var request = new GetCacheRequest(1, EthereumAddress.ZeroAddress, () =>
        {
            called = true;
            return "http://localhost";
        }, false);

        called.Should().BeFalse();
        request.ChainId.Should().Be(1);
        request.ERC20Service.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithAsyncRpcUrlFactory_ShouldNotInvokeFactory()
    {
        var called = false;

        var request = new GetCacheRequest(1, EthereumAddress.ZeroAddress, () =>
        {
            called = true;
            return Task.FromResult("http://localhost");
        }, false);

        called.Should().BeFalse();
        request.ChainId.Should().Be(1);
        request.ERC20Service.Should().NotBeNull();
    }
}