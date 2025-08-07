using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Net.Cache.DynamoDb.ERC20.DynamoDb.Models;

namespace Net.Cache.DynamoDb.ERC20.DynamoDb
{
    /// <summary>
    /// Provides access to ERC20 token metadata stored in DynamoDB.
    /// </summary>
    public interface IDynamoDbClient
    {
        /// <summary>
        /// Retrieves a token entry by its hash key.
        /// </summary>
        /// <param name="hashKey">The composite hash key of the token.</param>
        /// <param name="config">Optional DynamoDB load configuration.</param>
        /// <returns>The token entry if it exists; otherwise, <c>null</c>.</returns>
        public Task<Erc20TokenDynamoDbEntry?> GetErc20TokenAsync(HashKey hashKey, LoadConfig? config = null);

        /// <summary>
        /// Persists a token entry into DynamoDB.
        /// </summary>
        /// <param name="entry">The token entry to store.</param>
        /// <param name="config">Optional DynamoDB save configuration.</param>
        public Task SaveErc20TokenAsync(Erc20TokenDynamoDbEntry entry, SaveConfig? config = null);
    }
}
