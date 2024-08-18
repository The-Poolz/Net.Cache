using Newtonsoft.Json;

namespace Net.Cache.DynamoDb.ERC20.Models.Api
{
    public class ApiResponse
    {
        [JsonProperty("data")]
        public Data Data { get; set; } = null!;
    }
}