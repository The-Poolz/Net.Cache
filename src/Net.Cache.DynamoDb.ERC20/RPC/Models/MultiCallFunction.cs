using System.Linq;
using Nethereum.Contracts;
using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Net.Cache.DynamoDb.ERC20.RPC.Models
{
    [Function("multicall", "bytes[]")]
    public class MultiCallFunction : FunctionMessage
    {
        [Parameter("tuple[]", "calls", order: 1)]
        public MultiCall[] Calls { get; }

        public MultiCallFunction(IEnumerable<MultiCall> calls)
        {
            Calls = calls.ToArray();
        }

        public MultiCallFunction() : this(Enumerable.Empty<MultiCall>()) { }
    }
}
