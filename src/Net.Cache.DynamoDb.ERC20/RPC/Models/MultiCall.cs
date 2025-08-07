using Net.Web3.EthereumWallet;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Net.Cache.DynamoDb.ERC20.Rpc.Models
{
    /// <summary>
    /// Represents a single call item for the Multicall contract.
    /// </summary>
    public class MultiCall
    {
        /// <summary>
        /// Gets the address of the contract to call.
        /// </summary>
        [Parameter("address", "to", order: 1)]
        public string To { get; }

        /// <summary>
        /// Gets the encoded call data.
        /// </summary>
        [Parameter("bytes", "data", order: 2)]
        public byte[] Data { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiCall"/> class.
        /// </summary>
        /// <param name="to">The contract address to call.</param>
        /// <param name="data">The encoded function data.</param>
        public MultiCall(EthereumAddress to, byte[] data)
        {
            To = to;
            Data = data;
        }
    }
}
