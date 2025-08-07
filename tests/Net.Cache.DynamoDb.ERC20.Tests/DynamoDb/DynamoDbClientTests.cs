using Moq;
using Xunit;
using System.Numerics;
using FluentAssertions;
using Amazon.DynamoDBv2;
using Net.Web3.EthereumWallet;
using Amazon.DynamoDBv2.DataModel;
using Net.Cache.DynamoDb.ERC20.DynamoDb;
using Net.Cache.DynamoDb.ERC20.Rpc.Models;
using Net.Cache.DynamoDb.ERC20.DynamoDb.Models;

namespace Net.Cache.DynamoDb.ERC20.Tests.DynamoDb;

public class DynamoDbClientTests
{
    public class Constructor
    {
        [Fact]
        public void WhenContextIsNull_ShouldThrow()
        {
            var act = () => new DynamoDbClient((IDynamoDBContext)null!);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void WhenContextBuilderIsNull_ShouldThrow()
        {
            var act = () => new DynamoDbClient((IDynamoDBContextBuilder)null!);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void WhenDynamoDbClientIsNull_ShouldThrow()
        {
            var act = () => new DynamoDbClient((IAmazonDynamoDB)null!);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ShouldBuildContextFromBuilder()
        {
            Environment.SetEnvironmentVariable("AWS_REGION", "us-east-1");

            var mockBuilder = new Mock<IDynamoDBContextBuilder>();
            var mockContext = new DynamoDBContext(new AmazonDynamoDBClient());
            mockBuilder.Setup(b => b.Build()).Returns(mockContext);

            _ = new DynamoDbClient(mockBuilder.Object);

            mockBuilder.Verify(b => b.Build(), Times.Once);
        }
    }

    public class GetErc20TokenAsync
    {
        [Fact]
        public async Task ShouldCallLoadAsyncWithCorrectParameters()
        {
            var mockContext = new Mock<IDynamoDBContext>();
            var client = new DynamoDbClient(mockContext.Object);
            var hashKey = new HashKey(1, EthereumAddress.ZeroAddress);
            var token = new Erc20TokenData(EthereumAddress.ZeroAddress, "Token", "TKN", 2, new BigInteger(1000));
            var expectedEntry = new Erc20TokenDynamoDbEntry(hashKey, token);
            var config = new LoadConfig();
            mockContext
                .Setup(c => c.LoadAsync<Erc20TokenDynamoDbEntry>(hashKey.Value, config, CancellationToken.None))
                .ReturnsAsync(expectedEntry);

            var result = await client.GetErc20TokenAsync(hashKey, config);

            result.Should().Be(expectedEntry);
            mockContext.Verify(c => c.LoadAsync<Erc20TokenDynamoDbEntry>(hashKey.Value, config, CancellationToken.None), Times.Once);
        }
    }

    public class SaveErc20TokenAsync
    {
        [Fact]
        public async Task ShouldCallSaveAsyncWithCorrectParameters()
        {
            var mockContext = new Mock<IDynamoDBContext>();
            var client = new DynamoDbClient(mockContext.Object);
            var hashKey = new HashKey(1, EthereumAddress.ZeroAddress);
            var token = new Erc20TokenData(EthereumAddress.ZeroAddress, "Token", "TKN", 2, new BigInteger(1000));
            var entry = new Erc20TokenDynamoDbEntry(hashKey, token);
            var config = new SaveConfig();
            mockContext
                .Setup(c => c.SaveAsync(entry, config, CancellationToken.None))
                .Returns(Task.CompletedTask);

            await client.SaveErc20TokenAsync(entry, config);

            mockContext.Verify(c => c.SaveAsync(entry, config, CancellationToken.None), Times.Once);
        }
    }
}