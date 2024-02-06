# Net.Cache Repository

[![SonarCloud](https://sonarcloud.io/images/project_badges/sonarcloud-white.svg)](https://sonarcloud.io/summary/new_code?id=The-Poolz_Net.Cache)

[![CodeFactor](https://www.codefactor.io/repository/github/the-poolz/net.cache/badge)](https://www.codefactor.io/repository/github/the-poolz/net.cache)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=The-Poolz_Net.Cache&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=The-Poolz_Net.Cache)

## Overview

Welcome to the Net.Cache repository! This repository hosts two related .NET libraries, `Net.Cache` and `Net.Cache.DynamoDb`, which are designed to provide flexible and efficient caching solutions for .NET applications. 

`Net.Cache` is a general-purpose caching library, while `Net.Cache.DynamoDb` extends it with specific support for Amazon DynamoDB as a storage backend.

## Libraries

### Net.Cache

- **Description**: A generic caching library for .NET applications.
- **Key Features**: Provides a generic interface for storage providers and a concrete cache provider implementation.
- **Usage**: Ideal for applications requiring efficient data retrieval and caching mechanisms.
- **[Read More](https://github.com/The-Poolz/Net.Cache/tree/master/src/Net.Cache/README.md)**

### Net.Cache.DynamoDb

- **Description**: An extension of Net.Cache for integrating with Amazon DynamoDB.
- **Key Features**: Offers an abstract base class for creating DynamoDB storage providers.
- **Usage**: Best suited for applications using Amazon DynamoDB for key-value storage.
- **[Read More](https://github.com/The-Poolz/Net.Cache/tree/master/src/Net.Cache.DynamoDb/README.md)**

### Net.Cache.DynamoDb.ERC20

- **Description**: A specialized extension for caching ERC20 token information in Amazon DynamoDB.
- **Key Features**:
  - Caches ERC20 token details like name, symbol, decimals, and total supply.
  - Generates unique hash keys for efficient data retrieval.
  - Supports dynamic updates of token total supply in the cache.
- **Usage**: Ideal for blockchain-based applications or services that interact with ERC20 tokens and require efficient caching solutions.
- **[Read More](https://github.com/The-Poolz/Net.Cache/tree/master/src/Net.Cache.DynamoDb.ERC20/README.md)**
