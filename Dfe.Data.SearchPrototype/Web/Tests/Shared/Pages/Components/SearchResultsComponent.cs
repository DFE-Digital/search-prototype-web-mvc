using Dfe.Testing.Pages.DocumentQueryClient;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components.ValueObjects;
using Dfe.Testing.Pages.Pages.Components;
using Dfe.Testing.Pages.DocumentQueryClient.Selector;
using Dfe.Testing.Pages.DocumentQueryClient.Accessor;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;
public sealed class SearchResultsComponent : ComponentBase
{
    internal static IQuerySelector Container => new ElementSelector("#results");
    public SearchResultsComponent(IDocumentQueryClientAccessor documentQueryClientAccessor) : base(documentQueryClientAccessor)
    {

    }


    public string GetResultsHeading()
        => DocumentQueryClient.Query(
            new QueryCommand<string>(
                query: new ElementSelector("#search-results-count"),
                queryScope: Container,
                processor: (t) => t.Text.Trim()));

    public IEnumerable<EstablishmentSearchResult> GetResults()
        => DocumentQueryClient.QueryMany(new QueryCommand<EstablishmentSearchResult>(
                query: new ElementSelector("#establishment-search-results > ul"),
                queryScope: Container,
                processor: (t) =>
                {
                    return new(
                        name: t.GetChild(new ElementSelector("h4"))!.Text.Trim(),
                        urn: t.GetChild(new ElementSelector("li:nth-of-type(2) > span"))!.Text.Trim(),
                        //address: t.GetChild(new ElementSelector("li:nth-of-type(3)")).Text,
                        typeOfEstablishment: t.GetChild(new ElementSelector("li:nth-of-type(4) > span"))!.Text.Trim(),
                        status: t.GetChild(new ElementSelector("li:nth-of-type(5) > span"))!.Text.Trim(),
                        phase: t.GetChild(new ElementSelector("li:nth-of-type(6) > span"))!.Text.Trim());
                }
            ));
    /*
     *         Street = street;
        Locality = locality;
        Address3 = address3;
        Town = town;
        Postcode = postcode;
     */
}