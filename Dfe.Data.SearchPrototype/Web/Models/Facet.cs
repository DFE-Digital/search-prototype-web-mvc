namespace Dfe.Data.SearchPrototype.Web.Models;

public record Facet(
        string Name, // e.g., "Establishment status"
        FacetValue[] Values
    );
