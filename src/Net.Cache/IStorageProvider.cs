namespace Net.Cache;

public interface IStorageProvider<in TKey, TValue>
{
    void Store(TKey key, TValue value);
    bool TryGetValue(TKey key, out TValue value);
}