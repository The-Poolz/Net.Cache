using Net.Web3.EthereumWallet;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Net.Cache.DynamoDb.ERC20.RPC.Models
{
    public class MultiCall
    {
        [Parameter("address", "to", order: 1)]
        public string To { get; }

        [Parameter("bytes", "data", order: 2)]
        public byte[] Data { get; }

        public MultiCall(EthereumAddress to, byte[] data)
        {
            To = to;
            Data = data;
        }
    }
}
