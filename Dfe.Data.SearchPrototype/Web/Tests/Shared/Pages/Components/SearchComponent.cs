using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient;
using DfE.Data.SearchPrototype.Web.Tests.Shared.DocumentQueryClient.Accessor;
using DfE.Data.SearchPrototype.Web.Tests.Shared.Pages.Components.Input;

namespace DfE.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;

public sealed class SearchComponent : ComponentBase
{
    public SearchComponent(
        IDocumentQueryClientAccessor documentQueryClientAccessor
        ) : base(documentQueryClientAccessor)
    {
    }

    internal override IQuerySelector Container => new CssSelector("#search-establishments-form");

    public string GetHeading() =>
        DocumentQueryClient.Query(
            new QueryCommand<string>(
                    query: new CssSelector("#search-page-search-establishments-form-label"),
                    queryScope: Container,
                    processor: (t) => t.Text));

    public string GetSubheading()
        => DocumentQueryClient.Query(
            new QueryCommand<string>(
                query: new CssSelector("#searchKeyWord-hint"),
                queryScope: Container,
                processor: (t) => t.Text));

    public Input.Input GetSearchInput()
        => DocumentQueryClient.Query(
            new QueryCommand<Input.Input>(
                query: new CssSelector("#searchKeyWord"),
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
                query: new CssSelector("#no-results"),
                queryScope: Container,
                processor: (t) => t.Text.Trim()));
}