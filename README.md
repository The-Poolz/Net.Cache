# Net.Cache Repository

## Overview

Welcome to the Net.Cache repository! This repository hosts two related .NET libraries, `Net.Cache` and `Net.Cache.DynamoDb`, which are designed to provide flexible and efficient caching solutions for .NET applications. 

`Net.Cache` is a general-purpose caching library, while `Net.Cache.DynamoDb` extends it with specific support for Amazon DynamoDB as a storage backend.

## Libraries

### Net.Cache

- **Description**: A generic caching library for .NET applications.
- **Key Features**: Provides a generic interface for storage providers and a concrete cache provider implementation.
- **Usage**: Ideal for applications requiring efficient data retrieval and caching mechanisms.
- **[Read More](./Net.Cache/README.md)**

### Net.Cache.DynamoDb

- **Description**: An extension of Net.Cache for integrating with Amazon DynamoDB.
- **Key Features**: Offers an abstract base class for creating DynamoDB storage providers.
- **Usage**: Best suited for applications using Amazon DynamoDB for key-value storage.
- **[Read More](./Net.Cache.DynamoDb/README.md)**
