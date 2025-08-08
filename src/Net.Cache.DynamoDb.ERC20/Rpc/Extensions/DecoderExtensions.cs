using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Net.Cache.DynamoDb.ERC20.Rpc.Extensions
{
    /// <summary>
    /// Provides helper methods for decoding multicall responses.
    /// </summary>
    public static class DecoderExtensions
    {
        /// <summary>
        /// Decodes raw call data into the specified DTO type.
        /// </summary>
        /// <typeparam name="TFunctionOutputDTO">The DTO type to decode into.</typeparam>
        /// <param name="data">The raw byte data returned by the contract call.</param>
        /// <returns>The decoded DTO instance.</returns>
        public static TFunctionOutputDTO Decode<TFunctionOutputDTO>(this byte[] data) where TFunctionOutputDTO : IFunctionOutputDTO, new()
        {
            var dto = new TFunctionOutputDTO();
            return dto.DecodeOutput(data.ToHex());
        }
    }
}
