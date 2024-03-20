using Moq;
using Xunit;
using FluentAssertions;
using Net.Cache.Tests.Mock;

namespace Net.Cache.Tests;

public class CacheProviderTests
{
    private const string existKey = "key 1";
    private const string existValue = "value 1";
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

    public class TryGet
    {
        private readonly CacheProvider<string, string> cacheProvider = new(new MockStorageProvider());

        [Fact]
        internal void WhenKeyExists_ShouldReturnTrueAndExpectedValue()
        {
            var isExist = cacheProvider.TryGet(existKey, out var value);

            isExist.Should().BeTrue();
            value.Should().NotBeNull();
            value.Should().Be(existValue);
        }

        [Fact]
        internal void WhenKeyDoesNotExist_ShouldReturnFalseAndNullValue()
        {
            var isExist = cacheProvider.TryGet(notExistKey, out var value);

            isExist.Should().BeFalse();
            value.Should().BeNull();
        }

        [Fact]
        internal void WhenExceptionThrown_ShouldReturnFalseAndNullValue()
        {
            var provider = new Mock<IStorageProvider<string, string>>();
            var value = string.Empty;
            provider.Setup(x => x.TryGetValue(notExistKey, out value))
                .Throws<Exception>();
            var cache = new CacheProvider<string, string>(provider.Object);
            var isExist = cache.TryGet(notExistKey, out value);

            isExist.Should().BeFalse();
            value.Should().Be(string.Empty);
        }
    }

    public class Add
    {
        private readonly MockStorageProvider storageProvider;
        private readonly CacheProvider<string, string> cacheProvider;

        public Add()
        {
            storageProvider = new MockStorageProvider();
            cacheProvider = new CacheProvider<string, string>(storageProvider);
        }

        [Fact]
        internal void WhenKeyExists_ShouldThrowArgumentException()
        {
            var testCode = () => cacheProvider.Add(existKey, "value 1");

            testCode.Should().Throw<ArgumentException>()
                .WithMessage($"An item with the same key has already been added. Key: {existKey}");
        }

        [Fact]
        internal void WhenKeyDoesNotExist_ShouldItemHasBeenAdded()
        {
            const string expectedValue = "value 10";

            var testCode = () => cacheProvider.Add(notExistKey, expectedValue);

            testCode.Should().NotThrow();
            storageProvider.Storage[notExistKey].Should().Be(expectedValue);
        }
    }

    public class Delete
    {
        private readonly MockStorageProvider storageProvider;
        private readonly CacheProvider<string, string> cacheProvider;

        public Delete()
        {
            storageProvider = new MockStorageProvider();
            cacheProvider = new CacheProvider<string, string>(storageProvider);
        }

        [Fact]
        internal void WhenKeyExists_ShouldRemoveItem()
        {
            var testCode = () => cacheProvider.Delete(existKey);

            testCode.Should().NotThrow();
            storageProvider.Storage.ContainsKey(existKey).Should().BeFalse();
        }

        [Fact]
        internal void WhenKeyDoesNotExist_ShouldNothingHappened()
        {
            var testCode = () => cacheProvider.Delete(notExistKey);

            testCode.Should().NotThrow();
            storageProvider.Storage.ContainsKey(notExistKey).Should().BeFalse();
        }
    }

    public class Update
    {
        private const string newValue = "new value 1";
        private readonly MockStorageProvider storageProvider;
        private readonly CacheProvider<string, string> cacheProvider;

        public Update()
        {
            storageProvider = new MockStorageProvider();
            cacheProvider = new CacheProvider<string, string>(storageProvider);
        }

        [Fact]
        internal void WhenKeyExists_ShouldUpdateItem()
        {
            var testCode = () => cacheProvider.Update(existKey, newValue);

            testCode.Should().NotThrow();
            storageProvider.Storage[existKey].Should().Be(newValue);
        }

        [Fact]
        internal void WhenKeyDoesNotExist_ShouldNothingHappened()
        {
            var testCode = () => cacheProvider.Update(notExistKey, newValue);

            testCode.Should().NotThrow();
            storageProvider.Storage.ContainsKey(notExistKey).Should().BeFalse();
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

    public class ContainsKey
    {
        private readonly CacheProvider<string, string> cacheProvider = new(new MockStorageProvider());

        [Fact]
        internal void WhenKeyExists_ShouldReturnTrue()
        {
            var isExist = cacheProvider.ContainsKey(existKey);

            isExist.Should().BeTrue();
        }

        [Fact]
        internal void WhenKeyDoesNotExist_ShouldReturnFalse()
        {
            var isExist = cacheProvider.ContainsKey(notExistKey);

            isExist.Should().BeFalse();
        }
    }
}