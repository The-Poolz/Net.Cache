using System.Linq;
using Nethereum.Contracts;
using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Net.Cache.DynamoDb.ERC20.Rpc.Models
{
    /// <summary>
    /// Represents a multicall function message that aggregates multiple calls.
    /// </summary>
    [Function("multicall", "bytes[]")]
    public class MultiCallFunction : FunctionMessage
    {
        /// <summary>
        /// Gets the collection of calls to execute.
        /// </summary>
        [Parameter("tuple[]", "calls", order: 1)]
        public MultiCall[] Calls { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiCallFunction"/> class with the specified calls.
        /// </summary>
        /// <param name="calls">The calls to execute within the multicall.</param>
        public MultiCallFunction(IEnumerable<MultiCall> calls)
        {
            Calls = calls.ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiCallFunction"/> class with no calls.
        /// </summary>
        public MultiCallFunction() : this(Enumerable.Empty<MultiCall>()) { }
    }
}
