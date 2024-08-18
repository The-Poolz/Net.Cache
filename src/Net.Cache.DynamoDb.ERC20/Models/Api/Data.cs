using Newtonsoft.Json;
using System.Collections.Generic;

namespace Net.Cache.DynamoDb.ERC20.Models.Api
{
    public class Data
    {
        [JsonProperty("items")]
        public List<Item> Items { get; set; } = null!;
    }
}