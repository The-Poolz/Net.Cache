using Newtonsoft.Json;

namespace Net.Cache.DynamoDb.ERC20.Models.Api
{
    public class Item
    {
        [JsonProperty("contract_decimals")]
        public byte ContractDecimals { get; set; }

        [JsonProperty("contract_name")]
        public string ContractName { get; set; } = null!;

        [JsonProperty("contract_ticker_symbol")]
        public string ContractTickerSymbol { get; set; } = null!;

        [JsonProperty("total_supply")]
        public string TotalSupply { get; set; } = null!;
    }
}