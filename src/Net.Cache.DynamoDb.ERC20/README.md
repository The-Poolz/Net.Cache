# Net.Cache.DynamoDb.ERC20

## Overview

Net.Cache.DynamoDb.ERC20 is a specialized extension designed to facilitate the caching of ERC20 token information using Amazon DynamoDB.
It integrates seamlessly with Ethereum blockchain networks to provide efficient access and management of ERC20 token details, such as token name, symbol, decimals, and total supply.

## Features

- `ERC20 Token Information Caching`: Efficiently caches ERC20 token information, reducing the need for repeated blockchain queries.
- `Automatic Key Generation`: Generates unique hash keys for each token based on its chain ID and address, ensuring efficient data retrieval.
- `Flexible Initialization Options`: Supports initialization with custom or default DynamoDB client settings.
- `Update Total Supply`: Offers functionality to update the total supply of tokens in the cache dynamically.
- `Integration with Net.Cache.DynamoDb`: Builds upon the robust caching capabilities of Net.Cache.DynamoDb, providing a specific solution for ERC20 tokens.


## Getting Started

### Installation

To get started with Net.Cache.DynamoDb.ERC20, ensure that you have Net.Cache.DynamoDb installed.
Then, include Net.Cache.DynamoDb.ERC20 in your project

### Usage

Initialize ERC20CacheProvider

You can initialize the `ERC20CacheProvider` using the default constructor or by passing an instance of `ERC20StorageProvider` for more customized settings.

```csharp
var erc20CacheProvider = new ERC20CacheProvider();
```

Or with a custom storage provider:

```csharp
var customContext = new DynamoDBContext(customClient);
var erc20StorageProvider = new ERC20StorageProvider(customContext);
var erc20CacheProvider = new ERC20CacheProvider(erc20StorageProvider);
```

Caching ERC20 Token Information

To cache ERC20 token information, create a `GetCacheRequest` and use the `GetOrAdd` method of `ERC20CacheProvider`:

```csharp
var chainId = BigInteger.Parse("1"); // Ethereum mainnet
var contractAddress = new EthereumAddress("0x..."); // ERC20 token contract address
var rpcUrl = "https://mainnet.infura.io/v3/YOUR_INFURA_PROJECT_ID";

var request = new GetCacheRequest(chainId, contractAddress, rpcUrl);
var tokenInfo = erc20CacheProvider.GetOrAdd(contractAddress, request);

Console.WriteLine($"Token Name: {tokenInfo.Name}, Symbol: {tokenInfo.Symbol}");
```

This method retrieves the token information from the cache if it exists or fetches it from the blockchain and adds it to the cache otherwise.
