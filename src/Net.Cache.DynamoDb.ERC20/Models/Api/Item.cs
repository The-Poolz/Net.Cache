using Newtonsoft.Json;

namespace Net.Cache.DynamoDb.ERC20.Models.Api
{
    /// <summary>
    /// Represents an item containing ERC20 token information retrieved from an API response.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Gets or sets the number of decimals the ERC20 token uses.
        /// </summary>
        /// <value>The number of decimals for the ERC20 token.</value>
        [JsonProperty("contract_decimals")]
        public byte ContractDecimals { get; set; }

        /// <summary>
        /// Gets or sets the name of the ERC20 token.
        /// </summary>
        /// <value>The name of the ERC20 token.</value>
        [JsonProperty("contract_name")]
        public string ContractName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the ticker symbol of the ERC20 token.
        /// </summary>
        /// <value>The ticker symbol of the ERC20 token.</value>
        [JsonProperty("contract_ticker_symbol")]
        public string ContractTickerSymbol { get; set; } = null!;

        /// <summary>
        /// Gets or sets the total supply of the ERC20 token as a string.
        /// </summary>
        /// <value>The total supply of the ERC20 token.</value>
        [JsonProperty("total_supply")]
        public string TotalSupply { get; set; } = null!;
    }
}