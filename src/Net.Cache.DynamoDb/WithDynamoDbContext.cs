using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace Net.Cache.DynamoDb;

public class WithDynamoDbContext
{
    protected readonly Lazy<IDynamoDBContext> lazyContext;
    protected IDynamoDBContext Context => lazyContext.Value;

    public WithDynamoDbContext(IAmazonDynamoDB client)
    {
        lazyContext = new Lazy<IDynamoDBContext>(new DynamoDBContext(client));
    }

    public WithDynamoDbContext(IDynamoDBContext context)
    {
        lazyContext = new Lazy<IDynamoDBContext>(context);
    }
}