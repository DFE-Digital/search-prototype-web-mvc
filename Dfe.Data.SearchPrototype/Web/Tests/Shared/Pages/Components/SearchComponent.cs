using DfE.Tests.Pages.DocumentQueryClient;
using DfE.Tests.Pages.DocumentQueryClient.Accessor;
using DfE.Tests.Pages.DocumentQueryClient.Selector;
using DfE.Tests.Pages.Pages.Components;
using DfE.Tests.Pages.Pages.Components.Input;

namespace DfE.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;

public sealed class SearchComponent : ComponentBase
{
    internal static IQuerySelector Container => new ElementSelector("#search-establishments-form");

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
                    processor: (t) => t.Text));

    public string GetSubheading()
        => DocumentQueryClient.Query(
            new QueryCommand<string>(
                query: new ElementSelector("#searchKeyWord-hint"),
                queryScope: Container,
                processor: (t) => t.Text));

    public Input GetSearchInput()
        => DocumentQueryClient.Query(
            new QueryCommand<Input>(
                query: new ElementSelector("#searchKeyWord"),
                queryScope: Container,
                processor: (t) => new()
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
                processor: (t) => t.Text.Trim()));
}