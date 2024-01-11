# Net.Cache.DynamoDb

## Overview

Net.Cache.DynamoDb extends the capabilities of the Net.Cache library by integrating with Amazon DynamoDB.
This integration allows for the use of DynamoDB as a distributed, scalable, and highly available backend for caching operations.

## Features

- `DynamoDB Integration`: Leverages Amazon DynamoDB for storing and retrieving cache data.
- `Easy Configuration`: Offers various initialization methods for the DynamoDB client.
- `Seamless Compatibility`: Fully compatible with the Net.Cache library, providing DynamoDB as a storage option.

## Getting Started

### Installation

Install Net.Cache.DynamoDb via NuGet along with Net.Cache

### Defining DynamoDB Models
To use `DynamoDbStorageProvider`, you need to define models representing your DynamoDB tables:

```csharp
using Amazon.DynamoDBv2.DataModel;

[DynamoDBTable("MyTableName")]
public class MyTable
{
    [DynamoDBHashKey] // Partition key
    public string Id { get; set; }

    [DynamoDBProperty("MyCustomNameIfNeeded")] // Optional custom property name
    public string SomeProperty { get; set; }

    // Other properties as needed
}
```

### Usage

#### Initializing DynamoDbStorageProvider

Create an instance of `DynamoDbStorageProvider<TKey, TValue>` using one of the following methods:

1. Default Initialization:

```csharp
var dynamoDbProvider = new DynamoDbStorageProvider<int, MyTable>();
```

2. Custom DynamoDB Client:

```csharp
var customClient = new AmazonDynamoDBClient(...);
var dynamoDbProvider = new DynamoDbStorageProvider<int, MyTable>(customClient);
```

3. Custom DynamoDB Context:

```csharp
var customContext = new DynamoDBContext(customClient);
var dynamoDbProvider = new DynamoDbStorageProvider<int, MyTable>(customContext);
```

#### Integrating with Net.Cache

```csharp
var cache = new CacheProvider<int, MyTable>(dynamoDbProvider);
```

#### Storing and Retrieving Data

```csharp
// Store a value
cache.Store(1, new MyTable { Id = "1", SomeProperty = "Value" });

// Retrieve a value
if (cache.TryGetValue(1, out var myTableInstance)) {
    // Use myTableInstance
}
```