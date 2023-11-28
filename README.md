# Net.Cache

## Overview

Net.Cache is a C# library designed to provide efficient and easy-to-use caching solutions for .NET applications. It offers a generic interface and a concrete implementation for caching values by key, ensuring quick data retrieval and improved performance.

## Features

- **Generic Interface**: `IStorageProvider<TKey, TValue>` allows for a flexible storage mechanism that can be adapted to various storage systems.
- **Cache Provider**: `CacheProvider<TKey, TValue>` handles the caching logic, including storage and retrieval of values.
- **Extensibility**: Easily extendable with custom storage providers.

## Getting Started

To use Net.Cache in your project, follow these steps:

1. Install the package via NuGet.
2. Create an instance of `CacheProvider`, passing your storage provider and value factory function.
3. Use the `GetOrAdd` and `TryAdd` methods to interact with the cache.

## Example Usage

```csharp
// Define your storage provider
IStorageProvider<string, int> storageProvider = new YourStorageProvider();

// Create a CacheProvider instance
var cache = new CacheProvider<string, int>(storageProvider, key => ComputeValue(key));

// Retrieve or add value to cache
int value = cache.GetOrAdd("myKey");

// Add value to cache
bool added = cache.TryAdd("newKey", 42);
```

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
