using System;
using Amazon.DynamoDBv2;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Net.Cache.DynamoDb.ERC20.DynamoDb.Models;

namespace Net.Cache.DynamoDb.ERC20.DynamoDb
{
    /// <summary>
    /// Default implementation of <see cref="IDynamoDbClient"/> using the AWS SDK.
    /// </summary>
    public class DynamoDbClient : IDynamoDbClient
    {
        private readonly IDynamoDBContext _dynamoDbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamoDbClient"/> class.
        /// </summary>
        /// <param name="dynamoDbContext">The DynamoDB context to operate on.</param>
        public DynamoDbClient(IDynamoDBContext dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext ?? throw new ArgumentNullException(nameof(dynamoDbContext));
        }

        /// <summary>
        /// Initializes a new instance using a context builder.
        /// </summary>
        /// <param name="contextBuilder">Builder that produces a DynamoDB context.</param>
        public DynamoDbClient(IDynamoDBContextBuilder contextBuilder)
            : this((contextBuilder ?? throw new ArgumentNullException(nameof(contextBuilder))).Build())
        { }

        /// <summary>
        /// Initializes a new instance from a raw DynamoDB client.
        /// </summary>
        /// <param name="dynamoDb">The AWS DynamoDB client.</param>
        public DynamoDbClient(IAmazonDynamoDB dynamoDb)
            : this(
                new DynamoDBContextBuilder()
                    .WithDynamoDBClient(() => dynamoDb ?? throw new ArgumentNullException(nameof(dynamoDb)))
            )
        { }

        /// <summary>
        /// Initializes a new instance using default AWS configuration.
        /// </summary>
        public DynamoDbClient()
            : this(
                new DynamoDBContextBuilder()
                    .WithDynamoDBClient(() => new AmazonDynamoDBClient())
            )
        { }

        /// <inheritdoc cref="IDynamoDbClient.GetErc20TokenAsync"/>
        public async Task<Erc20TokenDynamoDbEntry?> GetErc20TokenAsync(HashKey hashKey, LoadConfig? config = null)
        {
            return await _dynamoDbContext
                .LoadAsync<Erc20TokenDynamoDbEntry>(hashKey.Value, config)
                .ConfigureAwait(false);
        }

        /// <inheritdoc cref="IDynamoDbClient.SaveErc20TokenAsync"/>
        public async Task SaveErc20TokenAsync(Erc20TokenDynamoDbEntry entry, SaveConfig? config = null)
        {
            await _dynamoDbContext
                .SaveAsync(entry, config)
                .ConfigureAwait(false);
        }
    }
}
