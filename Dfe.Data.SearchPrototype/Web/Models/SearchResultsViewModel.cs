namespace Dfe.Data.SearchPrototype.Web.Models;

/// <summary>
/// A view model representation of search results defining view logic predicated on the status of the results.
/// </summary>
public class SearchResultsViewModel
{
    /// <summary>
    /// View model representation of the available facets.
    /// </summary> 
    public List<Facet>? Facets { get; set; }
    /// <summary>
    /// View model representation of aggregated search results.
    /// </summary>
    public List<EstablishmentViewModel>? SearchItems { get; set; }
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

public class SearchRequestViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public string? SearchKeyword { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Dictionary<string, List<string>>? SelectedFacets { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool HasSelectedFacets() => SelectedFacets != null && SelectedFacets.Any();
}
