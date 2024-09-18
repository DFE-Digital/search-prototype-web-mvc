namespace Dfe.Data.SearchPrototype.Web.ViewModels;

/// <summary>
/// 
/// </summary>
/// <param name="Name"></param>
/// <param name="Values"></param>
public record Facet(
    string Name, // e.g., "Establishment status"
    List<FacetValue> Values
);
