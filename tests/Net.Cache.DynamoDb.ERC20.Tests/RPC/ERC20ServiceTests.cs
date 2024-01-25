using Moq;
using Xunit;
using Nethereum.Web3;
using FluentAssertions;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20.RPC;
using Nethereum.Contracts.Services;

namespace Net.Cache.DynamoDb.ERC20.Tests.RPC;

public class ERC20ServiceTests
{
    [Fact]
    internal void Ctor()
    {
        var eth = new Mock<IEthApiContractService>();
        var erc20 = new Mock<Nethereum.Contracts.Standards.ERC20.ERC20Service>(eth.Object).Object;

        eth.Setup(x => x.ERC20).Returns(erc20);

        var web3 = new Mock<IWeb3>();
        web3.Setup(x => x.Eth).Returns(eth.Object);

        var contractAddress = EthereumAddress.ZeroAddress;

        var erc20Service = new ERC20Service(web3.Object, contractAddress);

        erc20Service.Should().NotBeNull();
    }
}