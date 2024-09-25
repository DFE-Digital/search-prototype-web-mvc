namespace Dfe.Data.SearchPrototype.Web.Models;

/// <summary>
/// A view model representation of search requests defining view logic predicated on the status of user input.
/// </summary>
public class SearchRequest
{
    /// <summary>
    /// The search keyword provisioned by binding to user input.
    /// </summary>
    public string? SearchKeyword { get; set; }

    /// <summary>
    /// The dictionary of selected facets (grouped by facet name key) provisioned
    /// by binding to user input.
    /// </summary>
    public Dictionary<string, List<string>>? SelectedFacets { get; set; }

    /// <summary>
    /// Conditionally checks if any facet values have been selected.
    /// </summary>
    public bool HasSelectedFacets => SelectedFacets?.Count > 0;

    /// <summary>
    /// Conditionally checks if a search keyword has been submitted.
    /// </summary>
    public bool HasSearchKeyWord => !string.IsNullOrWhiteSpace(SearchKeyword);
}