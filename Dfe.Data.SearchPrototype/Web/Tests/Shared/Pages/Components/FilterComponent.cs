using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components.ValueObject;
using Dfe.Testing.Pages.DocumentQueryClient;
using Dfe.Testing.Pages.DocumentQueryClient.Accessor;
using Dfe.Testing.Pages.DocumentQueryClient.Pages;
using Dfe.Testing.Pages.DocumentQueryClient.Pages.GDSComponents.Buttons;
using Dfe.Testing.Pages.DocumentQueryClient.Pages.GDSComponents.Form;
using Dfe.Testing.Pages.DocumentQueryClient.Selector;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;

public sealed class FilterComponent : PagePartBase
{
    private readonly FormFactory _formFactory;

    internal static IElementSelector FiltersContainer => new ElementSelector("#filters-container");

    private static QueryRequest FacetValueByValue(FacetValue facetValue) =>
        new()
        {
            Query = new ElementSelector($"input[value={facetValue.Value}]"),
            Scope = FiltersContainer
        };

    private static QueryRequest SubmitFiltersButton =>
        new()
        {
            Query = new ElementSelector("#filters-button"),
            Scope = FiltersContainer
        };
    private static QueryRequest ClearFiltersButton =>
        new()
        {
            Query = new ElementSelector("#clearFilters"),
            Scope = FiltersContainer
        };

    public FilterComponent(
        IDocumentQueryClientAccessor documentQueryClientAccessor,
        FormFactory formComponent) : base(documentQueryClientAccessor)
    {
        _formFactory = formComponent;
    }

    public IEnumerable<Facet> GetDisplayedFacets()
        => _formFactory.Get().FieldSets
                .Select(
                    (fieldSet) => new Facet(
                        Name: fieldSet.Legend,
                        FacetValues: fieldSet.Checkboxes.Select(
                            (checkbox) => new FacetValue(checkbox.Label, checkbox.Value))));

    public IEnumerable<Button> GetFilterButtons() => _formFactory.Get().Buttons;

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
