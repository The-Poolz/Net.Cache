using Net.Web3.EthereumWallet;

namespace Net.Cache.DynamoDb.ERC20.Models.Api
{
    public class ApiERC20ServiceConfig
    {
        public ApiERC20ServiceConfig(string apiKey, long chainId, EthereumAddress contractAddress, string apiUrl)
        {
            ApiKey = apiKey;
            ChainId = chainId;
            ContractAddress = contractAddress;
            ApiUrl = apiUrl;
        }

        public string ApiKey { get; set; }
        public long ChainId { get; set; }
        public EthereumAddress ContractAddress { get; set; }
        public string ApiUrl { get; set; }
    }
}