using Net.Cache.DynamoDb.Attributes;

namespace Net.Cache.DynamoDb.Models;

[AcceptableKeyTypes(typeof(int), typeof(string))]
public class DynamoDbTable<TKey>
    where TKey : IEquatable<TKey>
{
    public TKey PrimaryKey { get; set; }
}