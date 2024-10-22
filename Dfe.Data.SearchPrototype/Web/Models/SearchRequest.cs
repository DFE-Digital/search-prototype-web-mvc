namespace Dfe.Data.SearchPrototype.Web.Models;

/// <summary>
/// The search requests defining model bound view response logic predicated on the status of user input.
/// </summary>
public class SearchRequest
{
    /// <summary>
    /// The search keyword provisioned by binding to user input.
    /// </summary>
    public string? SearchKeyword { get; set; }
    public int? PageNumber { get; set; }

    /// <summary>
    /// The dictionary of selected facets (grouped by facet name key) provisioned
    /// by binding to user input.
    /// </summary>
    public Dictionary<string, List<string>>? SelectedFacets
    {
        get
        {
            if (ClearFilters) _selectedFacets = null;
            return _selectedFacets;
        }
        set { _selectedFacets = value; }
    }

    private Dictionary<string, List<string>>? _selectedFacets;
    public bool ClearFilters { get; set; }
}