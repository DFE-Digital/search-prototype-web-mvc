using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components.ValueObject;
using Dfe.Testing.Pages.DocumentQueryClient;
using Dfe.Testing.Pages.DocumentQueryClient.Accessor;
using Dfe.Testing.Pages.DocumentQueryClient.Pages;
using Dfe.Testing.Pages.DocumentQueryClient.Pages.Components.Form;
using Dfe.Testing.Pages.DocumentQueryClient.Selector;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;

public sealed class FilterComponent : PagePartBase
{
    private readonly FormComponentFactory _formComponent;

    internal static IElementSelector FiltersContainer => new ElementSelector("#filters-container");

    private static QueryRequest FacetValueByValue(FacetValue facetValue) =>
        new(
            query: new ElementSelector($"input[value={facetValue.Value}]"),
            scope: FiltersContainer);

    private static QueryRequest SubmitFiltersButton =>
        new(
            query: new ElementSelector("#filters-button"),
            scope: FiltersContainer);

    private static QueryRequest ClearFiltersButton =>
        new(
            query: new ElementSelector("#clearFilters"),
            scope: FiltersContainer);

    private static QueryRequest Facets =>
        new(
            query: new ElementSelector(".govuk-fieldset"),
            scope: FiltersContainer);

    public FilterComponent(
        IDocumentQueryClientAccessor documentQueryClientAccessor,
        FormComponentFactory formComponent) : base(documentQueryClientAccessor)
    {
        _formComponent = formComponent;
    }

    public IEnumerable<Facet> GetDisplayedFacets()
        => _formComponent.GetMany().Single().FieldSets
                .Select(
                    (fieldSet) => new Facet(
                        Name: fieldSet.Legend,
                        FacetValues: fieldSet.Checkboxes.Select(
                            (checkbox) => new FacetValue(checkbox.Label, checkbox.Value))));

    public FilterComponent ApplyFacetValue(FacetValue applyFacet)
    {
        DocumentQueryClient.Run(
            FacetValueByValue(applyFacet), (part) => part.Click());
        return this;
    }

    public FilterComponent SubmitFilters()
    {
        DocumentQueryClient.Run(SubmitFiltersButton, (part) => part.Click());
        return this;
    }

    public FilterComponent ClearFilters()
    {
        DocumentQueryClient.Run(ClearFiltersButton, (part) => part.Click());
        return this;
    }
}
