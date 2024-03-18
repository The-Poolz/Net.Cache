using Xunit;
using FluentAssertions;
using Net.Cache.Tests.Mock;

namespace Net.Cache.Tests;

public class CacheProviderTests
{
    private const string existKey = "key 1";
    private const string notExistKey = "key 10";

    public class Ctor
    {
        [Fact]
        internal void PassIEnumerable()
        {
            var storageProviders = new List<IStorageProvider<string, string>>()
            {
                new MockStorageProvider(),
                new InMemoryStorageProvider<string, string>()
            };
            var cacheProvider = new CacheProvider<string, string>(storageProviders);

            cacheProvider.Should().NotBeNull();
        }

        [Fact]
        internal void PassParams()
        {
            var storageProvider = new MockStorageProvider();

            var cacheProvider = new CacheProvider<string, string>(storageProvider);

            cacheProvider.Should().NotBeNull();
        }
    }

    public class Get
    {
        private readonly CacheProvider<string, string> cacheProvider = new(new MockStorageProvider());

        [Fact]
        internal void WhenKeyExists_ShouldReturnTheValue()
        {
            var actualValue = cacheProvider.Get(existKey);

            actualValue.Should().Be(MockStorageProvider.DefaultStorage[existKey]);
        }

        [Fact]
        internal void WhenKeyDoesNotExist_ShouldThrowKeyNotFoundException()
        {
            var testCode = () => cacheProvider.Get(notExistKey);

            testCode.Should().Throw<KeyNotFoundException>()
                .WithMessage($"The value associated with the key '{notExistKey}' was not found in any storage provider.");
        }
    }

    public class GetOrAdd
    {
        private readonly CacheProvider<string, string> cacheProvider = new(new MockStorageProvider());

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void WhenKeyExist_ShouldReturnExistValue(bool useParameterlessFunc)
        {
            var existValue = useParameterlessFunc ?
                cacheProvider.GetOrAdd(existKey, () => "this value will not be added") :
                cacheProvider.GetOrAdd(existKey, _ => "this value will not be added");

            existValue.Should().Be(MockStorageProvider.DefaultStorage[existKey]);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void WhenKeyDoesNotExist_ShouldAddValue(bool useParameterlessFunc)
        {
            const string expectedValue = "value 10";

            var addedValue = useParameterlessFunc ?
                cacheProvider.GetOrAdd(notExistKey, () => expectedValue) :
                cacheProvider.GetOrAdd(notExistKey, _ => expectedValue);

            addedValue.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void WhenKeyNotExistInPrimaryStorageProvider_ShouldReturnExistValueFromSecondaryProvider_ShouldAddedValueToPrimaryProvider(bool useParameterlessFunc)
        {
            const string expectedValue = "value 10";
            var primaryProvider = new MockStorageProvider();
            var secondaryProvider = new InMemoryStorageProvider<string, string>();
            secondaryProvider.Store(notExistKey, expectedValue);
            var cache = new CacheProvider<string, string>(primaryProvider, secondaryProvider);

            var existValueFromSecondaryProvider = useParameterlessFunc ?
                cache.GetOrAdd(notExistKey, () => "this value will not be added") :
                cache.GetOrAdd(notExistKey, _ => "this value will not be added");

            existValueFromSecondaryProvider.Should().Be(expectedValue);
            primaryProvider.Storage[notExistKey].Should().Be(expectedValue);
        }
    }
}