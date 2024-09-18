namespace Dfe.Data.SearchPrototype.Web.ViewModels;

/// <summary>
/// 
/// </summary>
/// <param name="Value"></param>
/// <param name="Count"></param>
/// <param name="IsSelected"></param>
public record FacetValue(
    string Value,       // e.g., "Open, but proposed to close"
    long? Count,         // e.g., 150
    bool IsSelected     // e.g., true
);