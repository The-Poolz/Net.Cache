using Moq;
using Xunit;
using FluentAssertions;

namespace Net.Cache.Tests;

public class CacheProviderTests
{
    [Fact]
    public void GetOrCache_UsingInMemoryStorageProvider()
    {
        var storageProvider = new InMemoryStorageProvider<string, string>();
        var cacheProvider = new CacheProvider<string, string>(storageProvider);

        var cat = cacheProvider.GetOrAdd("cat", () => "Meow");
        var dog = cacheProvider.GetOrAdd("dog", _ => "Woof woof");
        cat.Should().Be("Meow");
        dog.Should().Be("Woof woof");

        var existCatVoice = cacheProvider.GetOrAdd("cat", _ => "method should returns initial description");
        var existDogVoice = cacheProvider.GetOrAdd("dog", _ => "method should returns initial description");
        existCatVoice.Should().Be("Meow");
        existDogVoice.Should().Be("Woof woof");
    }

    [Fact]
    public void GetOrAdd_WithParameterlessFactory_CachesValue()
    {
        var key = "testKey";
        var expectedValue = "testValue";
        var storageMock = new Mock<IStorageProvider<string, string>>();
        storageMock.Setup(s => s.TryGetValue(key, out expectedValue)).Returns(true);

        var cacheProvider = new CacheProvider<string, string>(storageMock.Object);
        var result = cacheProvider.GetOrAdd(key, () => "newValue");

        result.Should().Be(expectedValue);
        storageMock.Verify(s => s.Store(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void GetOrAdd_WithFactoryAndParameters_CachesValue()
    {
        var key = "testKey";
        var expectedValue = "testValue";
        var storageMock = new Mock<IStorageProvider<string, string>>();
        storageMock.Setup(s => s.TryGetValue(key, out expectedValue)).Returns(false);

        var cacheProvider = new CacheProvider<string, string>(storageMock.Object);
        var result = cacheProvider.GetOrAdd(key, args => (string)args[0], expectedValue);

        result.Should().Be(expectedValue);
        storageMock.Verify(s => s.Store(key, expectedValue), Times.Once);
    }

    [Fact]
    public void GetOrAdd_StoresValueInPrimaryStorage()
    {
        var key = "testKey";
        var value = "testValue";
        var storageMock = new Mock<IStorageProvider<string, string>>();
        storageMock.Setup(s => s.TryGetValue(key, out value)).Returns(false);

        var cacheProvider = new CacheProvider<string, string>(storageMock.Object);
        cacheProvider.GetOrAdd(key, () => value);

        storageMock.Verify(s => s.Store(key, value), Times.Once);
    }

    [Fact]
    public void GetOrAdd_RetrievesValueFromSecondaryStorage()
    {
        var key = "testKey";
        var value = "testValue";
        var primaryStorageMock = new Mock<IStorageProvider<string, string>>();
        var secondaryStorageMock = new Mock<IStorageProvider<string, string>>();
        secondaryStorageMock.Setup(s => s.TryGetValue(key, out value)).Returns(true);

        var cacheProvider = new CacheProvider<string, string>(primaryStorageMock.Object, secondaryStorageMock.Object);
        var result = cacheProvider.GetOrAdd(key, () => "newValue");

        result.Should().Be(value);
        primaryStorageMock.Verify(p => p.Store(key, value), Times.Once);
        secondaryStorageMock.Verify(s => s.Store(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}