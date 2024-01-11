# Net.Cache

## Overview

Net.Cache is a comprehensive caching library for .NET, designed to offer versatile tools for cache management.
The library focuses on facilitating key-value based caching operations and provides a default implementation with `InMemoryStorageProvider`.
It allows for easy integration and extension, enabling developers to use different caching strategies while maintaining a consistent interface.

## Features

- `Generic Caching Mechanism`: Supports any data type for keys and values, requiring keys to be equatable and non-nullable.
- `Default In-Memory Caching`: Includes `InMemoryStorageProvider` for out-of-the-box in-memory caching.
- `Customizable Storage Providers`: Easily extend or implement custom storage providers to suit various caching needs.
- `Safe Value Retrieval`: Utilize `TryGetValue` for safe retrieval, reducing the risk of exceptions with missing keys.
- `Dynamic Cache Management`: Efficiently handle caching with the ability to dynamically add values when not present.

## Getting Started

### Installation

Install `Net.Cache` via the NuGet package manager or clone the repository into your project.

### Usage

#### Implementing Custom Storage Providers

Create custom storage providers by implementing the `IStorageProvider<TKey, TValue>` interface.
Alternatively, use the provided `InMemoryStorageProvider<TKey, TValue>`.

```csharp
var inMemoryStorage = new InMemoryStorageProvider<int, string>();
```

#### Using CacheProvider

Manage your caching logic with `CacheProvider<TKey, TValue>`, which can handle multiple storage providers including custom ones.

```csharp
var cache = new CacheProvider<int, string>(inMemoryStorage);
```

#### Storing and Retrieving Values
Easily store and retrieve values using keys.

```csharp
// Storing a value
cache.Store(1, "Hello World");

// Retrieving a value
if (cache.TryGetValue(1, out var value)) {
    Console.WriteLine(value); // Outputs: Hello World
}
```

#### Adding Values Dynamically

Leverage the GetOrAdd method to add values to the cache dynamically if they don't already exist.

```csharp
string value = cache.GetOrAdd(2, () => "New Value");
```