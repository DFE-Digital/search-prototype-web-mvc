using Dfe.Testing.Pages.DocumentQueryClient.Accessor;
using Dfe.Testing.Pages.DocumentQueryClient.Selector;
using Dfe.Testing.Pages.DocumentQueryClient;
using Dfe.Testing.Pages.DocumentQueryClient.Pages;
using Dfe.Testing.Pages.DocumentQueryClient.Pages.GDSComponents.Inputs;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;

public sealed class SearchComponent : PagePartBase
{
    internal static ElementSelector Container => new("#search-establishments-form");
    internal static QueryRequest SearchInput => new() { Query = new ElementSelector("#searchKeyWord"), Scope = Container };
    internal static QueryRequest Heading => new() { Query = new ElementSelector("#search-page-search-establishments-form-label"), Scope = Container };
    internal static QueryRequest SubHeading => new()
    {
        Query = new ElementSelector("#searchKeyWord-hint"),
        Scope = Container
    };

    internal static QueryRequest NoSearchResultsHeading => new()
    {
        Query = new ElementSelector("#no-results"),
        Scope = Container
    };

    internal static QueryRequest SearchButton => new()
    {
        Query = new ElementSelector("#search"),
        Scope = Container
    };

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

    public SearchComponent SubmitSearch()
    {
        DocumentQueryClient.Run(SearchButton, (button) => button.Click());
        return this;
    }
}