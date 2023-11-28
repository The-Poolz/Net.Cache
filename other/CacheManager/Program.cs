namespace LazyCache.Example;

internal static class Program
{
    private static void Main()
    {
        var manager = new CachingService();

        var dogKey = Guid.NewGuid().ToString();
        manager.Add(dogKey, "Dog");

        var value = manager.GetOrAdd(dogKey, _ => "Cat");

        Console.WriteLine(value); // Dog

        value = manager.GetOrAdd(Guid.NewGuid().ToString(), _ => "Cat");

        Console.WriteLine(value); // Cat
    }
}