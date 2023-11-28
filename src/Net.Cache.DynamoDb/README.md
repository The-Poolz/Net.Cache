# Net.Cache.DynamoDb

## Overview

Net.Cache.DynamoDb is a specialized extension of the Net.Cache library, providing an abstract base class for DynamoDB storage providers.
It's specifically designed for applications that use Amazon DynamoDB as a backend storage for caching.

## Features

- **DynamoDB Integration**: Seamlessly integrates with Amazon DynamoDB for efficient storage and retrieval.
- **Abstract Base Class**: `AbstractDynamoDbStorageProvider<TKey, TValue>` allows for easy implementation of custom DynamoDB storage logic.
- **Flexibility**: Can be customized to suit various key-value storage requirements with DynamoDB.

## Getting Started

To use Net.Cache.DynamoDb in your project, follow these steps:

1. Install the package via NuGet.
2. Implement `AbstractDynamoDbStorageProvider` with your key and value types.
3. Use this implementation with `CacheProvider` from Net.Cache for caching.

## Example Usage

```csharp
public class MyDynamoDbStorageProvider : AbstractDynamoDbStorageProvider<string, MyObject>
{
    public MyDynamoDbStorageProvider() : base("MyDynamoDbTable") {}

    public override void Store(string key, MyObject value) { /* Implementation */ }

    public override bool TryGetValue(string key, out MyObject value) { /* Implementation */ }
}

// Usage with CacheProvider
var dynamoDbStorageProvider = new MyDynamoDbStorageProvider();
var cache = new CacheProvider<string, MyObject>(dynamoDbStorageProvider, key => new MyObject());

// Retrieve or add value to cache
MyObject value = cache.GetOrAdd("myKey");
```
