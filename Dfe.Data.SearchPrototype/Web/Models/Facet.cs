namespace Dfe.Data.SearchPrototype.Web.Models;

/// <summary>
/// Value object used to encapsulate the Facet view model.
/// </summary>
/// <param name="Name">
/// The name of the facet.
/// </param>
/// <param name="Values">
/// The collection of <see cref="FacetValue"/> associated with the named facet.
/// </param>
public record Facet(
    string Name, // e.g., "Establishment status"
    List<FacetValue> Values
);
