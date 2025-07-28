using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Net.Cache.DynamoDb.ERC20.RPC.Extensions
{
    public static class DecoderHelperExtensions
    {
        public static TFunctionOutputDTO Decode<TFunctionOutputDTO>(this byte[] data) where TFunctionOutputDTO : IFunctionOutputDTO, new()
        {
            var dto = new TFunctionOutputDTO();
            return dto.DecodeOutput(data.ToHex());
        }
    }
}
