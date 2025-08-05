using Amazon.DynamoDBv2;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Net.Cache.DynamoDb.ERC20.DynamoDb.Models;

namespace Net.Cache.DynamoDb.ERC20.DynamoDb
{
    public class DynamoDbClient : IDynamoDbClient
    {
        private readonly IDynamoDBContext _dynamoDbContext;

        public DynamoDbClient(IDynamoDBContext dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext;
        }

        public DynamoDbClient(IDynamoDBContextBuilder contextBuilder)
            : this(contextBuilder.Build())
        { }

        public DynamoDbClient(IAmazonDynamoDB dynamoDb)
            : this(
                new DynamoDBContextBuilder()
                    .WithDynamoDBClient(() => dynamoDb)
            )
        { }

        public DynamoDbClient()
            : this(
                new DynamoDBContextBuilder()
                    .WithDynamoDBClient(() => new AmazonDynamoDBClient())
            )
        { }

        public async Task<Erc20TokenDynamoDbEntry?> GetErc20TokenAsync(string hashKey, LoadConfig? config = null)
        {
            return await _dynamoDbContext.LoadAsync<Erc20TokenDynamoDbEntry>(hashKey, config);
        }

        public Task SaveErc20TokenAsync(Erc20TokenDynamoDbEntry entry, SaveConfig? config = null)
        {
            return _dynamoDbContext.SaveAsync(entry, config);
        }
    }
}
