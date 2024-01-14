using Moq;
using Xunit;
using FluentAssertions;

namespace Net.Cache.Tests;

public class CacheProviderTests
{
    private const string key = "testKey";
    private readonly StorageMock storageMock = new();
    private string Value => storageMock.value;
    public static IEnumerable<object[]> AnimalSoundsTestData()
    {
        yield return new object[] { "cat", "Meow" };
        yield return new object[] { "dog", "Woof woof" };
        yield return new object[] { "cow", "Moo" };
        yield return new object[] { "duck", "Quack" };
    }

    [Theory]
    [MemberData(nameof(AnimalSoundsTestData))]
    public void GetOrCache_AnimalSounds(string animal, string expectedSound)
    {
        var storageProvider = new InMemoryStorageProvider<string, string>();
        var cacheProvider = new CacheProvider<string, string>(storageProvider);

        var actualSound = cacheProvider.GetOrAdd(animal, () => expectedSound);
        actualSound.Should().Be(expectedSound);

        var existingSound = cacheProvider.GetOrAdd(animal, () => throw new OperationCanceledException("method should return initial description"));
        existingSound.Should().Be(expectedSound);
    }

    public static IEnumerable<object[]> GetOrAddTestData()
    {
        // Test case: GetOrAdd_WithParameterlessFactory_CachesValue
        yield return new object[] { true, Times.Never(), new Func<string>(() => "newValue"), null! };

        // Test case: GetOrAdd_StoresValueInPrimaryStorage
        yield return new object[] { false, Times.Once(), new Func<string>(() => "testValue"), null! };

        // Test case: GetOrAdd_WithFactoryAndParameters_CachesValue
        yield return new object[] { false, Times.Once(), new Func<object[], string>(args => (string)args[0]), new object[] { "testValue" } };
    }

    [Theory]
    [MemberData(nameof(GetOrAddTestData))]
    public void GetOrAdd_CombinedTheory(
        bool returns,
        Times timesExpectedStoreCalled,
        Delegate factory,
        object[] factoryArgs)
    {
        var cacheProvider = storageMock.GetMockCacheProvider(returns);
        var result = factory switch
        {
            Func<string> parameterlessFactory => cacheProvider.GetOrAdd(key, parameterlessFactory),
            Func<object[], string> parameterizedFactory => cacheProvider.GetOrAdd(key, parameterizedFactory, factoryArgs),
            _ => throw new InvalidOperationException("Invalid factory delegate type.")
        };

        result.Should().Be(Value);
        storageMock.Verify(timesExpectedStoreCalled);
    }


    [Fact]
    public void GetOrAdd_RetrievesValueFromSecondaryStorage()
    {
        var primaryStorageMock = new Mock<IStorageProvider<string, string>>();

        var cacheProvider = new CacheProvider<string, string>(primaryStorageMock.Object, storageMock.GetMockCache(key, true));
        var result = cacheProvider.GetOrAdd(key, () => "newValue");

        result.Should().Be(Value);
        primaryStorageMock.Verify(p => p.Store(key, Value), Times.Once);
        storageMock.Verify(Times.Never());
    }
}