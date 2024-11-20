using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components.ValueObject;
using Dfe.Testing.Pages.DocumentQueryClient;
using Dfe.Testing.Pages.DocumentQueryClient.Accessor;
using Dfe.Testing.Pages.DocumentQueryClient.Pages.Components;
using Dfe.Testing.Pages.DocumentQueryClient.Pages.Components.CheckboxInput;
using Dfe.Testing.Pages.DocumentQueryClient.Selector;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;

public sealed class FilterComponent : ComponentBase
{
    private readonly CheckboxWithLabelComponent _checkboxWithLabelComponent;

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
        CheckboxWithLabelComponent checkboxWithLabelComponent) : base(documentQueryClientAccessor)
    {
        _checkboxWithLabelComponent = checkboxWithLabelComponent;
    }

    public IEnumerable<Facet> GetDisplayedFacets()
         => DocumentQueryClient.QueryMany(
                args: Facets,
                mapper: (facet)
                    => new Facet(
                        Name: facet.GetChild(new ElementSelector("legend"))!.Text.Trim(),
                        FacetValues:
                        _checkboxWithLabelComponent.GetCheckboxesFromPart(facet)
                            .Select((checkboxWithLabel) => new FacetValue(checkboxWithLabel.Label, checkboxWithLabel.Value))));

    public FilterComponent ApplyFacet(FacetValue applyFacet)
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
