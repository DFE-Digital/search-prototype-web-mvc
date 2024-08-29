namespace Dfe.Data.SearchPrototype.Web.Models;

/// <summary>
/// A view model representation of search results defining view logic predicated on the status of the results.
/// </summary>
public class SearchResultsViewModel
{
    /// <summary>
    /// View model representation of the available facets.
    /// </summary> 
    // public List<Facet>? Facets { get; set; } = new();
    public List<Facet>? Facets { get; set; } = new()
    {
        new Facet("Establishment status", new FacetValue[]
        {
            new("Open, but proposed to close", 150, true),
            new("Open", 2543, true),
            new("Closed", 2001, false),
            new("Proposed to open", 73, true),
            new("Proposed to close", 25, false),
            new("Closed by the department", 3, false)
        }),
        new Facet("Type of establishment", new FacetValue[]
        {
            new("Academy converter", 42, true),
            new("Academy sponsor led", 14, true),
            new("Academy special converter", 6263, false),
            new("Academy special sponsor led", 2555, true),
            new("Academy special", 2551, false),
            new("Academy 16-19 sponsor led", 773, false)
        }),
    };
    
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
    
    
    public record Facet(
        string Name, // e.g., "Establishment status"
        FacetValue[] Values
    );

    public record FacetValue(
        string Value,       // e.g., "Open, but proposed to close"
        int Count,          // e.g., 150
        bool IsSelected     // e.g., true
    );
}
