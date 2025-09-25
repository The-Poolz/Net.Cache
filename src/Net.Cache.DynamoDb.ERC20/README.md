# Net.Cache.DynamoDb.ERC20

## Overview

Net.Cache.DynamoDb.ERC20 is a specialized extension designed to facilitate the caching of ERC20 token information using Amazon DynamoDB.
It integrates seamlessly with Ethereum blockchain networks to provide efficient access and management of ERC20 token details, such as token name, symbol, decimals, and total supply.

## Features

- `ERC20 Token Information Caching`: Efficiently caches ERC20 token information, reducing the need for repeated blockchain queries.
- `Automatic Key Generation`: Generates unique hash keys for each token based on its chain ID and address, ensuring efficient data retrieval.
- `Flexible Initialization Options`: Supports initialization with custom or default DynamoDB client and RPC service settings.
- `In-Memory Layer`: Uses an in-memory cache alongside DynamoDB for faster repeated access.
- `Integration with Net.Cache.DynamoDb`: Builds upon the robust caching capabilities of Net.Cache.DynamoDb, providing a specific solution for ERC20 tokens.


## Getting Started

### Installation

To get started with Net.Cache.DynamoDb.ERC20, ensure that you have Net.Cache.DynamoDb installed.
Then, include Net.Cache.DynamoDb.ERC20 in your project

### Usage

#### Initialize `Erc20CacheService`

You can initialize the `Erc20CacheService` using the default constructor or by passing custom implementations of `IDynamoDbClient` and `IErc20ServiceFactory`:

```csharp
var cacheService = new Erc20CacheService();
```

Or with custom dependencies:

```csharp
var context = new DynamoDBContext(customClient);
var dynamoDbClient = new DynamoDbClient(context);
var erc20ServiceFactory = new Erc20ServiceFactory();
var cacheService = new Erc20CacheService(dynamoDbClient, erc20ServiceFactory);
```

#### Caching ERC20 Token Information

To cache ERC20 token information, create a `HashKey` and use one of the `GetOrAddAsync` overloads of `Erc20CacheService`.

The overload that accepts RPC and multicall address factories is useful when you only need to supply connection metadata:

```csharp
var chainId = 1L; // Ethereum mainnet
var contractAddress = new EthereumAddress("0x..."); // ERC20 token contract address
var hashKey = new HashKey(chainId, contractAddress);

var tokenInfo = await cacheService.GetOrAddAsync(
    hashKey,
    rpcUrlFactory: () => Task.FromResult("https://mainnet.infura.io/v3/YOUR_INFURA_PROJECT_ID"),
    multiCallFactory: () => Task.FromResult(new EthereumAddress("0x...multicall"))
);

Console.WriteLine($"Token Name: {tokenInfo.Name}, Symbol: {tokenInfo.Symbol}");
```

If you need to control the entire `IWeb3` client creation, you can use the overload that accepts a `Func<Task<IWeb3>>` alongside the multicall factory:

```csharp
var tokenInfo = await cacheService.GetOrAddAsync(
    hashKey,
    web3Factory: () => Task.FromResult((IWeb3)new Web3("https://mainnet.infura.io/v3/YOUR_INFURA_PROJECT_ID")),
    multiCallFactory: () => Task.FromResult(new EthereumAddress("0x...multicall"))
);

Console.WriteLine($"Token Name: {tokenInfo.Name}, Symbol: {tokenInfo.Symbol}");
```

Both overloads retrieve the token information from the cache if it exists, or fetch it from the blockchain and store it in both the DynamoDB table and the in-memory cache otherwise.