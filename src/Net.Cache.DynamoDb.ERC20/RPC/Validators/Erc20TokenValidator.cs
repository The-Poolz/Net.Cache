using System.Numerics;
using FluentValidation;
using Net.Cache.DynamoDb.ERC20.RPC.Models;

namespace Net.Cache.DynamoDb.ERC20.RPC.Validators
{
    internal class Erc20TokenValidator : AbstractValidator<Erc20Token>
    {
        public Erc20TokenValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is missing.");

            RuleFor(x => x.Symbol)
                .NotEmpty()
                .WithMessage("Symbol is missing.");

            RuleFor(x => x.Decimals)
                .GreaterThanOrEqualTo((byte)0)
                .WithMessage("Decimals is invalid.");

            RuleFor(x => x.TotalSupply)
                .GreaterThanOrEqualTo(BigInteger.Zero)
                .WithMessage("TotalSupply is negative.");
        }
    }
}
