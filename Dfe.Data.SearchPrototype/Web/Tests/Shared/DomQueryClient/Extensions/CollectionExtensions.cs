namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient.Extensions;

internal static class CollectionExtensions
{
    internal static IEnumerable<T> ThrowIfNullOrEmpty<T>(this IEnumerable<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection);
        if (!collection.Any())
        {
            throw new ArgumentException("Collection is empty");
        }
        return collection;
    }

    internal static IEnumerable<T> ThrowIfMultiple<T>(this IEnumerable<T> collection)
    {
        int collectionCount = collection.Count();
        if (collectionCount > 1)
        {
            throw new ArgumentException($"Collection count: {collectionCount} is more than 1");
        }
        return collection;
    }
}