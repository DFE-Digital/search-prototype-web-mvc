namespace Dfe.Data.SearchPrototype.Web.Models;

public record FacetValue(
    string Value,       // e.g., "Open, but proposed to close"
    int Count,          // e.g., 150
    bool IsSelected     // e.g., true
);
