using Xunit;
using System.Numerics;
using FluentAssertions;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20.Rpc.Models;
using Net.Cache.DynamoDb.ERC20.Rpc.Validators;

namespace Net.Cache.DynamoDb.ERC20.Tests.Rpc.Validators;

public class Erc20TokenValidatorTests
{
    public class Validate
    {
        private readonly Erc20TokenValidator validator = new();

        [Fact]
        public void WhenTokenIsValid_ShouldReturnValid()
        {
            var token = new Erc20TokenData(EthereumAddress.ZeroAddress, "Token", "TKN", 18, BigInteger.Zero);

            var result = validator.Validate(token);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void WhenNameMissing_ShouldReturnError()
        {
            var token = new Erc20TokenData(EthereumAddress.ZeroAddress, "", "TKN", 18, BigInteger.Zero);

            var result = validator.Validate(token);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Name" && e.ErrorMessage == "Name is missing.");
        }

        [Fact]
        public void WhenSymbolMissing_ShouldReturnError()
        {
            var token = new Erc20TokenData(EthereumAddress.ZeroAddress, "Token", "", 18, BigInteger.Zero);

            var result = validator.Validate(token);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Symbol" && e.ErrorMessage == "Symbol is missing.");
        }

        [Fact]
        public void WhenTotalSupplyIsNegative_ShouldReturnError()
        {
            var token = new Erc20TokenData(EthereumAddress.ZeroAddress, "Token", "TKN", 18, BigInteger.MinusOne);

            var result = validator.Validate(token);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "TotalSupply" && e.ErrorMessage == "TotalSupply is negative.");
        }
    }
}