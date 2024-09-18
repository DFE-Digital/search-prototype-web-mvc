namespace Dfe.Data.SearchPrototype.Web.ViewModels;

public class SearchRequest
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
    public bool HasSelectedFacets() => SelectedFacets?.Count > 0;
}