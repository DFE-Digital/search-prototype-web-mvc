using Dfe.Testing.Pages.DocumentQueryClient.Accessor;
using Dfe.Testing.Pages.DocumentQueryClient.Selector;
using Dfe.Testing.Pages.DocumentQueryClient;
using Dfe.Testing.Pages.Pages.Components;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components.ValueObject;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;
public sealed class SearchResultsComponent : ComponentBase
{
    internal static ElementSelector Container => new("#results");
    public SearchResultsComponent(IDocumentQueryClientAccessor documentQueryClientAccessor) : base(documentQueryClientAccessor)
    {

    }

    public string GetResultsHeading()
        => DocumentQueryClient.Query(
            new QueryArgs(
                query: new ElementSelector("#search-results-count"), scope: Container),
                mapper: (documentPart) => documentPart.Text.Trim());

    public IEnumerable<EstablishmentSearchResult> GetResults()
        => DocumentQueryClient.QueryMany(
            args: new QueryArgs(
                query: new ElementSelector("#establishment-search-results > ul"), scope: Container),
            mapper: (documentPart) 
                => new EstablishmentSearchResult(
                        Name: documentPart.GetChild(new ElementSelector("h4"))!.Text.Trim(),
                        Urn: documentPart.GetChild(new ElementSelector("li:nth-of-type(2) > span"))!.Text.Trim(),
                        TypeOfEstablishment: documentPart.GetChild(new ElementSelector("li:nth-of-type(4) > span"))!.Text.Trim(),
                        Status: documentPart.GetChild(new ElementSelector("li:nth-of-type(5) > span"))!.Text.Trim(),
                        Phase: documentPart.GetChild(new ElementSelector("li:nth-of-type(6) > span"))!.Text.Trim()));
}