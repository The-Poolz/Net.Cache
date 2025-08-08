using Moq;
using Xunit;
using Nethereum.ABI;
using Nethereum.Web3;
using System.Numerics;
using FluentAssertions;
using Nethereum.RPC.Eth.DTOs;
using Net.Web3.EthereumWallet;
using Nethereum.Contracts.Services;
using Net.Cache.DynamoDb.ERC20.Rpc;
using Net.Cache.DynamoDb.ERC20.Rpc.Models;
using Nethereum.Contracts.ContractHandlers;
using Net.Cache.DynamoDb.ERC20.Rpc.Exceptions;

namespace Net.Cache.DynamoDb.ERC20.Tests.Rpc;

public class Erc20ServiceTests
{
    public class Constructor
    {
        [Fact]
        public void WhenWeb3Null_ShouldThrow()
        {
            var act = () => new Erc20Service(null!, EthereumAddress.ZeroAddress);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void WhenMultiCallNull_ShouldThrow()
        {
            var web3 = Mock.Of<IWeb3>();
            var act = () => new Erc20Service(web3, null!);
            act.Should().Throw<ArgumentNullException>();
        }
    }

    public class GetErc20TokenAsync
    {
        [Fact]
        public async Task WhenTokenNull_ShouldThrow()
        {
            var handler = new Mock<IContractQueryHandler<MultiCallFunction>>();
            var service = CreateService(handler.Object);
            var act = async () => await service.GetErc20TokenAsync(null!);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task WhenResponseValid_ShouldReturnToken()
        {
            var response = BuildResponse("Token", "TKN", 18, new BigInteger(1000));
            var handlerMock = new Mock<IContractQueryHandler<MultiCallFunction>>();
            handlerMock
                .Setup(h => h.QueryAsync<List<byte[]>>(It.IsAny<string>(), It.IsAny<MultiCallFunction>(), It.IsAny<BlockParameter>()))
                .ReturnsAsync(response);
            var service = CreateService(handlerMock.Object);

            var result = await service.GetErc20TokenAsync(EthereumAddress.ZeroAddress);

            result.Address.Should().Be(new EthereumAddress(EthereumAddress.ZeroAddress));
            result.Name.Should().Be("Token");
            result.Symbol.Should().Be("TKN");
            result.Decimals.Should().Be(18);
            result.TotalSupply.Should().Be(new BigInteger(1000));
        }

        [Fact]
        public async Task WhenTokenInvalid_ShouldThrow()
        {
            var response = BuildResponse(string.Empty, "TKN", 18, new BigInteger(1000));
            var handlerMock = new Mock<IContractQueryHandler<MultiCallFunction>>();
            handlerMock
                .Setup(h => h.QueryAsync<List<byte[]>>(It.IsAny<string>(), It.IsAny<MultiCallFunction>(), It.IsAny<BlockParameter>()))
                .ReturnsAsync(response);
            var service = CreateService(handlerMock.Object);

            var act = async () => await service.GetErc20TokenAsync(EthereumAddress.ZeroAddress);

            await act.Should().ThrowAsync<Erc20QueryException>()
                .WithMessage("*[ERC20*Name is missing.*");
        }
    }

    private static Erc20Service CreateService(IContractQueryHandler<MultiCallFunction> handler)
    {
        var web3Mock = new Mock<IWeb3>();
        var ethMock = new Mock<IEthApiContractService>();
        ethMock.Setup(e => e.GetContractQueryHandler<MultiCallFunction>()).Returns(handler);
        web3Mock.SetupGet(w => w.Eth).Returns(ethMock.Object);
        return new Erc20Service(web3Mock.Object, EthereumAddress.ZeroAddress);
    }

    private static List<byte[]> BuildResponse(string name, string symbol, byte decimals, BigInteger supply)
    {
        var abiEncode = new ABIEncode();
        return
        [
            abiEncode.GetABIEncoded(new ABIValue("string", name)),
            abiEncode.GetABIEncoded(new ABIValue("string", symbol)),
            abiEncode.GetABIEncoded(new ABIValue("uint8", decimals)),
            abiEncode.GetABIEncoded(new ABIValue("uint256", supply))
        ];
    }
}
