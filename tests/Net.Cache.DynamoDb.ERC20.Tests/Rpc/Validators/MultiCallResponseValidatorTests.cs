using Xunit;
using FluentAssertions;
using Net.Cache.DynamoDb.ERC20.Rpc.Validators;

namespace Net.Cache.DynamoDb.ERC20.Tests.Rpc.Validators;

public class MultiCallResponseValidatorTests
{
    public class Validate
    {
        [Fact]
        public void WhenCountIsUnexpected_ShouldReturnError()
        {
            var validator = new MultiCallResponseValidator(2);
            var response = new List<byte[]> { new byte[] { 1 } };

            var result = validator.Validate(response);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "MultiCall" && e.ErrorMessage == "MultiCall returned unexpected number of results.");
        }

        [Fact]
        public void WhenCallReturnsNoData_ShouldReturnError()
        {
            var validator = new MultiCallResponseValidator(2);
            var response = new List<byte[]> { new byte[] { 1 }, Array.Empty<byte>() };

            var result = validator.Validate(response);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Symbol" && e.ErrorMessage == "Symbol call returned no data.");
        }

        [Fact]
        public void WhenAllCallsReturnData_ShouldBeValid()
        {
            var validator = new MultiCallResponseValidator(2);
            var response = new List<byte[]> { new byte[] { 1 }, new byte[] { 2 } };

            var result = validator.Validate(response);

            result.IsValid.Should().BeTrue();
        }
    }
}