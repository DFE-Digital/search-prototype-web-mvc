namespace Dfe.Data.SearchPrototype.Web.ViewModels;

/// <summary>
/// A view model representation of search results defining view logic predicated on the status of the results.
/// </summary>
public class SearchResults
{
    /// <summary>
    /// View model representation of the available facets.
    /// </summary>
    public List<Facet>? Facets { get; set; }
    /// <summary>
    /// View model representation of aggregated search results.
    /// </summary>
    public List<Establishment>? SearchItems { get; set; }
    /// <summary>
    /// Property determining whether we have at least one search result.
    /// </summary>
    public bool HasResults => SearchResultsCount >= 1;
    /// <summary>
    /// Property determining whether we have more than one search result.
    /// </summary>
    public bool HasMoreThanOneResult => SearchResultsCount > 1;
    /// <summary>
    /// Property determining the number of search results.
    /// </summary>
    public int SearchResultsCount => SearchItems?.Count ?? 0;
}
