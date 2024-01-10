using Xunit;
using FluentAssertions;

namespace Net.Cache.Tests;

public class CacheProviderTests
{
    [Fact]
    public void GetOrCache_ShouldAddValueToCache_IfKeyNotExists()
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
}