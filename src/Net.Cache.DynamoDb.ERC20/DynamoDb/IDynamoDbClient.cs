using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Net.Cache.DynamoDb.ERC20.DynamoDb.Models;

namespace Net.Cache.DynamoDb.ERC20.DynamoDb
{
    public interface IDynamoDbClient
    {
        public Task<Erc20TokenDynamoDbEntry?> GetErc20TokenAsync(string hashKey, LoadConfig? config = null);
        public Task SaveErc20TokenAsync(Erc20TokenDynamoDbEntry entry, SaveConfig? config = null);
    }
}
