using AngleSharp.Dom;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components.ValueObject;
using Dfe.Testing.Pages.DocumentQueryClient;
using Dfe.Testing.Pages.DocumentQueryClient.Accessor;
using Dfe.Testing.Pages.DocumentQueryClient.Selector;
using Dfe.Testing.Pages.Pages.Components;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;

public sealed class FilterComponent : ComponentBase
{
    internal static IElementSelector FiltersContainer => new ElementSelector("#filters-container");

    private static QueryArgs FacetValueByValue(FacetValue facetValue)
        => new(
            query: new ElementSelector($"input[value={facetValue.Value}]"),
            scope: FiltersContainer);

    private static QueryArgs SubmitFiltersButton =>
        new(
            query: new ElementSelector("#filters-button"),
            scope: FiltersContainer);

    public FilterComponent(IDocumentQueryClientAccessor documentQueryClientAccessor) : base(documentQueryClientAccessor)
    {
    }

    public IEnumerable<Facet> GetDisplayedFacets()
    {
        return DocumentQueryClient.QueryMany<Facet>(
                new QueryArgs(
                    query: new ElementSelector(".govuk-fieldset"),
                    scope: FiltersContainer),
                (part) =>
                {
                    return new Facet(
                        Name: part.GetChild(new ElementSelector("legend"))!.Text.Trim(),
                        FacetValues: 
                            (part.GetChildren(new ElementSelector(".govuk-checkboxes__item")) ?? throw new ArgumentNullException("could not find checkboxes"))
                                .Select(
                                // TODO could map this into an IEnumerable<Checkbox> with labels in there for easier mapping into the FacetValue?
                                (checkboxWrapper) => new FacetValue(
                                    Label: checkboxWrapper.GetChild(new ElementSelector(".govuk-checkboxes__label"))!.Text!,
                                    Value: 
                                        (checkboxWrapper.GetChild(new ElementSelector(".govuk-checkboxes__input")) ?? throw new ArgumentNullException("could not find input"))
                                                .GetAttribute("value")!))
                                .ToList());
                });
    }



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
}
