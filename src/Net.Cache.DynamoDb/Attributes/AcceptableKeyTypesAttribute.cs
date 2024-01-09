namespace Net.Cache.DynamoDb.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class AcceptableKeyTypesAttribute : Attribute
{
    public Type[] AcceptedTypes { get; private set; }

    public AcceptableKeyTypesAttribute(params Type[] acceptedTypes)
    {
        AcceptedTypes = acceptedTypes;
    }
}