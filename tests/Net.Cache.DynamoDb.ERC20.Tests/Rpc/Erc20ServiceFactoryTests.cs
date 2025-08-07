using Moq;
using Xunit;
using Nethereum.Web3;
using FluentAssertions;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20.Rpc;

namespace Net.Cache.DynamoDb.ERC20.Tests.Rpc;

public class Erc20ServiceFactoryTests
{
    public class Create
    {
        [Fact]
        public void ShouldReturnErc20Service()
        {
            var web3 = new Mock<IWeb3>();
            var factory = new Erc20ServiceFactory();

            var service = factory.Create(web3.Object, EthereumAddress.ZeroAddress);

            service.Should().BeOfType<Erc20Service>();
        }

        [Fact]
        public void WhenDependencyNull_ShouldThrow()
        {
            var factory = new Erc20ServiceFactory();

            var act1 = () => factory.Create(null!, EthereumAddress.ZeroAddress);
            var act2 = () => factory.Create(new Mock<IWeb3>().Object, null!);

            act1.Should().Throw<ArgumentNullException>();
            act2.Should().Throw<ArgumentNullException>();
        }
    }
}