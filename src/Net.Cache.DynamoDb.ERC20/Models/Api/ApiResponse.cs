using Newtonsoft.Json;

namespace Net.Cache.DynamoDb.ERC20.Models.Api
{
    /// <summary>
    /// Represents the data container for ERC20 token information retrieved from an API response.
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Gets or sets the list of items containing ERC20 token information.
        /// </summary>
        /// <value>A list of <see cref="Item"/> objects that hold the details of ERC20 tokens.</value>
        [JsonProperty("data")]
        public Data Data { get; set; } = null!;
    }
}