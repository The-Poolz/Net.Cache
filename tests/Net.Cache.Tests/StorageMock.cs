using FluentAssertions;
using Moq;

namespace Net.Cache.Tests;

public class StorageMock
{
    private readonly Mock<IStorageProvider<string, string>> storageMock = new();
    public static string value = "testValue";
    public CacheProvider<string, string> GetMockCacheProvider(bool returns)
    {
        storageMock.Setup(s => s.TryGetValue(It.IsAny<string>(), out value!)).Returns(returns);
        return new CacheProvider<string, string>(storageMock.Object);
    }
    public IStorageProvider<string, string> GetMockCache(string key, bool returns)
    {
        storageMock.Setup(s => s.TryGetValue(key, out value!)).Returns(returns);
        return storageMock.Object;
    }
    public void Verify(string result, Times times)
    {
        Verify(times);
        Verify(result);
    }
    public void Verify(Times times) => storageMock.Verify(s => s.Store(It.IsAny<string>(), It.IsAny<string>()), times);
    public void Verify(string result) => result.Should().Be(value);
}
