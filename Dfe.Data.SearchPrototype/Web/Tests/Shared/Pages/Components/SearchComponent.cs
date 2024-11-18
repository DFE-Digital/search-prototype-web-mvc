using Dfe.Testing.Pages.DocumentQueryClient.Accessor;
using Dfe.Testing.Pages.DocumentQueryClient.Selector;
using Dfe.Testing.Pages.DocumentQueryClient;
using Dfe.Testing.Pages.Pages.Components.Inputs;
using Dfe.Testing.Pages.Pages.Components;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;

public sealed class SearchComponent : ComponentBase
{
    internal static ElementSelector Container => new("#search-establishments-form");
    internal static QueryArgs SearchInput => new(query: new ElementSelector("#searchKeyWord"), scope: Container);
    internal static QueryArgs Heading => new(
        query: new ElementSelector("#search-page-search-establishments-form-label"),
        scope: Container);
    internal static QueryArgs SubHeading => new(
        query: new ElementSelector("#searchKeyWord-hint"),
        scope: Container);

    internal static QueryArgs NoSearchResultsHeading => new(
        query: new ElementSelector("#no-results"),
        scope: Container);


    public SearchComponent(
        IDocumentQueryClientAccessor documentQueryClientAccessor,
        SearchResultsComponent searchResultsComponent) : base(documentQueryClientAccessor)
    {
        ArgumentNullException.ThrowIfNull(searchResultsComponent);
        SearchResults = searchResultsComponent;
    }

    public SearchResultsComponent SearchResults { get; }

    public string GetHeading() => DocumentQueryClient.Query(Heading, mapper: (t) => t.Text);
    public string GetSubheading() => DocumentQueryClient.Query(SubHeading, mapper: (t) => t.Text);
    public Input GetSearchInput()
        => DocumentQueryClient.Query(SearchInput, mapper: (t) => new Input()
        {
            Name = t.GetAttribute("name"),
            Value = t.GetAttribute("value"),
            PlaceHolder = t.GetAttribute("placeholder"),
            Type = t.GetAttribute("type")
        });

    public string GetNoSearchResultsMessage() => DocumentQueryClient.Query(NoSearchResultsHeading, mapper: (t) => t.Text);

    public SearchComponent SearchForEstablishmentWith(string term)
    {
        DocumentQueryClient.Run(SearchInput, (input) => input.Text = term);
        return this;
    }
}