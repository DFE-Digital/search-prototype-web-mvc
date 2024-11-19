namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components.ValueObject;

public record Facet(string Name, IEnumerable<FacetValue> FacetValues);
public record FacetValue(string Label, string Value);