using FluentValidation;
using System.Collections.Generic;

namespace Net.Cache.DynamoDb.ERC20.Rpc.Validators
{
    internal class MultiCallResponseValidator : AbstractValidator<IReadOnlyList<byte[]>>
    {
        public MultiCallResponseValidator(int expectedCount)
        {
            RuleFor(x => x).Custom((list, context) =>
            {
                if (list.Count != expectedCount)
                {
                    context.AddFailure("MultiCall", "MultiCall returned unexpected number of results.");
                    return;
                }

                for (var i = 0; i < list.Count; i++)
                {
                    var data = list[i];
                    if (data == null || data.Length == 0)
                    {
                        var field = GetFieldName(i);
                        context.AddFailure(field, $"{field} call returned no data.");
                    }
                }
            });
        }

        private static string GetFieldName(int index) => index switch
        {
            0 => "Name",
            1 => "Symbol",
            2 => "Decimals",
            3 => "TotalSupply",
            _ => $"Call[{index}]"
        };
    }
}
