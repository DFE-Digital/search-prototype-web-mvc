namespace DfE.Tests.Pages.Extensions;

internal static class CollectionExtensions
{
    internal static IEnumerable<T> ThrowIfNullOrEmpty<T>(this IEnumerable<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection);
        return !collection.Any() ? throw new ArgumentException("Collection is empty") : collection;
    }

    internal static IEnumerable<T> ThrowIfMultiple<T>(this IEnumerable<T> collection)
    {
        int collectionCount = collection.Count();
        return collectionCount > 1 ? throw new ArgumentException($"Collection count: {collectionCount} is more than 1") : collection;
    }
}