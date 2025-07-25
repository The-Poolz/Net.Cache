﻿using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System.Diagnostics.CodeAnalysis;

namespace Net.Cache.DynamoDb
{
    /// <summary>
    /// Provides an implementation of the <see cref="IStorageProvider{TKey, TValue}"/> for Amazon DynamoDB.
    /// This class allows for the storage and retrieval of values using DynamoDB as the underlying data store.
    /// </summary>
    /// <typeparam name="TKey">The type of keys used for identifying values. Must be equatable and non-nullable.</typeparam>
    /// <typeparam name="TValue">The type of values to be stored. This type is a class.</typeparam>
    public class DynamoDbStorageProvider<TKey, TValue> : IStorageProvider<TKey, TValue>
        where TKey : IEquatable<TKey>
        where TValue : class
    {
        protected const string EmptyString = "";
        protected readonly string tableName;
        protected readonly Lazy<IDynamoDBContext> lazyContext;
        protected IDynamoDBContext Context => lazyContext.Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamoDbStorageProvider{TKey, TValue}"/> class using the default Amazon DynamoDB client.
        /// This constructor is useful for quick setups where the default client configuration is sufficient.
        /// </summary>
        /// <param name="tableName">The name of the DynamoDB table to be used. If not specified or empty, the default table name from model is used.</param>
        public DynamoDbStorageProvider(string tableName = EmptyString)
            : this(new AmazonDynamoDBClient(), tableName)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamoDbStorageProvider{TKey, TValue}"/> class with the specified DynamoDB client.
        /// This constructor allows for more control over the DynamoDB client configuration.
        /// </summary>
        /// <param name="client">The DynamoDB client to be used for database operations.</param>
        /// <param name="tableName">The name of the DynamoDB table to be used. If not specified or empty, the default table name from model is used.</param>
        public DynamoDbStorageProvider(IAmazonDynamoDB client, string tableName = EmptyString)
        {
            lazyContext = new Lazy<IDynamoDBContext>(new DynamoDBContextBuilder()
                .WithDynamoDBClient(() => client)
                .Build()
            );
            this.tableName = tableName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamoDbStorageProvider{TKey, TValue}"/> class with a pre-configured DynamoDB context.
        /// This constructor provides the most flexibility, allowing the use of a custom-configured DynamoDB context.
        /// </summary>
        /// <param name="context">The DynamoDB context to be used for database operations.</param>
        /// <param name="tableName">The name of the DynamoDB table to be used. If not specified or empty, the default table name from model is used.</param>
        public DynamoDbStorageProvider(IDynamoDBContext context, string tableName = EmptyString)
        {
            lazyContext = new Lazy<IDynamoDBContext>(context);
            this.tableName = tableName;
        }

        /// <inheritdoc cref="IStorageProvider{TKey, TValue}.Store(TKey, TValue)"/>
        public virtual void Store(TKey key, TValue value)
        {
            Context.SaveAsync(value, OperationConfig<SaveConfig>())
                .GetAwaiter()
                .GetResult();
        }

        /// <inheritdoc cref="IStorageProvider{TKey, TValue}.TryGetValue(TKey, out TValue)"/>
        public virtual bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            value = null;
            try
            {
                value = Context.LoadAsync<TValue>(key, OperationConfig<LoadConfig>())
                    .GetAwaiter()
                    .GetResult();

                return value != null;
            }
            catch (AmazonDynamoDBException)
            {
                throw;
            }
            catch
            {
                return false;
            }
        }

        /// <inheritdoc cref="IStorageProvider{TKey, TValue}.Remove(TKey)"/>
        public void Remove(TKey key) => Context.DeleteAsync<TValue>(key)
            .GetAwaiter()
            .GetResult();

        /// <inheritdoc cref="IStorageProvider{TKey, TValue}.Update(TKey, TValue)"/>
        public void Update(TKey key, TValue value) => Context.SaveAsync(value, OperationConfig<SaveConfig>())
            .GetAwaiter()
            .GetResult();

        /// <inheritdoc cref="IStorageProvider{TKey, TValue}.ContainsKey(TKey)"/>
        public bool ContainsKey(TKey key) => Context.LoadAsync<TValue>(key, OperationConfig<LoadConfig>())
                .GetAwaiter()
                .GetResult() != null;

        protected TConfig? OperationConfig<TConfig>() where TConfig : BaseOperationConfig, new()
        {
            return string.IsNullOrWhiteSpace(tableName) ? null : new TConfig
            {
                OverrideTableName = tableName
            };
        }
    }
}
