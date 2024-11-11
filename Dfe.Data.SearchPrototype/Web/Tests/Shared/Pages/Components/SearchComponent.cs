using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient;
using DfE.Data.SearchPrototype.Web.Tests.Shared.DocumentQueryClient.Accessor;

namespace DfE.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;

public sealed class SearchComponent
{
    private readonly IDocumentQueryClientAccessor _documentQueryClientAccessor;
    private readonly IQuerySelector Container = new CssSelector("#search-establishments-form");

    public SearchComponent(IDocumentQueryClientAccessor documentQueryClientAccessor)
    {
        _documentQueryClientAccessor = documentQueryClientAccessor;
    }

    public string GetHeading() =>
        _documentQueryClientAccessor.DocumentQueryClient.Query(
            new QueryCommand<string>(
                    query: new CssSelector("#search-page-search-establishments-form-label"),
                    queryScope: Container,
                    processor: (t) => t.Text));

    public string GetSubheading()
        => _documentQueryClientAccessor.DocumentQueryClient.Query(
            new QueryCommand<string>(
                    query: new CssSelector("#searchKeyWord-hint"),
                    queryScope: Container,
                    processor: (t) => t.Text));
}