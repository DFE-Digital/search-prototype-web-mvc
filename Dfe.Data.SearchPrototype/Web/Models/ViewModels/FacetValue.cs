namespace Dfe.Data.SearchPrototype.Web.Models.ViewModels;

/// <summary>
/// Value object used to encapsulate the facet value view model.
/// </summary>
/// <param name="Value">
/// The value associated with the named facet.
/// /param>
/// <param name="Count">
/// The number of matching values derived on the underlying search.
/// </param>
/// <param name="IsSelected">
/// Allows selection criteria to be prescribed for re-binding.
/// </param>
public record FacetValue(
    string Value,       // e.g., "Open, but proposed to close"
    long? Count,        // e.g., 150
    bool IsSelected     // e.g., true
);