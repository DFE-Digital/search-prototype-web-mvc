using Dfe.Testing.Pages.DocumentQueryClient.Accessor;
using Dfe.Testing.Pages.DocumentQueryClient.Selector;
using Dfe.Testing.Pages.DocumentQueryClient;
using Dfe.Testing.Pages.Pages.Components.Inputs;
using Dfe.Testing.Pages.Pages.Components;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;

public sealed class SearchComponent : ComponentBase
{
    internal static ElementSelector Container => new("#search-establishments-form");

    public SearchComponent(
        IDocumentQueryClientAccessor documentQueryClientAccessor,
        SearchResultsComponent searchResultsComponent) : base(documentQueryClientAccessor)
    {
        ArgumentNullException.ThrowIfNull(searchResultsComponent);
        SearchResults = searchResultsComponent;
    }

    public SearchResultsComponent SearchResults { get; }

    public string GetHeading() =>
        DocumentQueryClient.Query(
            new QueryCommand<string>(
                    query: new ElementSelector("#search-page-search-establishments-form-label"),
                    queryScope: Container,
                    Mapper: (t) => t.Text));

    public string GetSubheading()
        => DocumentQueryClient.Query(
            new QueryCommand<string>(
                query: new ElementSelector("#searchKeyWord-hint"),
                queryScope: Container,
                Mapper: (t) => t.Text));

    public Input GetSearchInput()
        => DocumentQueryClient.Query(
            new QueryCommand<Input>(
                query: new ElementSelector("#searchKeyWord"),
                queryScope: Container,
                Mapper: (t) => new()
                {
                    Name = t.GetAttribute("name"),
                    Value = t.GetAttribute("value"),
                    PlaceHolder = t.GetAttribute("placeholder"),
                    Type = t.GetAttribute("type")
                }
                ));

    public string GetNoSearchResultsMessage()
        => DocumentQueryClient.Query(
            new QueryCommand<string>(
                query: new ElementSelector("#no-results"),
                queryScope: Container,
                Mapper: (t) => t.Text.Trim()));
}