using Dfe.Data.SearchPrototype.Web.Models.ViewModels;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient;
using DfE.Data.SearchPrototype.Web.Tests.Shared.DocumentQueryClient.Accessor;

namespace DfE.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;

public sealed class SearchComponent : ComponentBase
{
    public SearchComponent(
        IDocumentQueryClientAccessor documentQueryClientAccessor,
        SearchResultsComponent searchResultsComponent
        ) : base(documentQueryClientAccessor)
    {
        ArgumentNullException.ThrowIfNull(searchResultsComponent);
        SearchResults = searchResultsComponent;
    }

    internal override IQuerySelector Container => new ElementSelector("#search-establishments-form");

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

    public Input.Input GetSearchInput()
        => DocumentQueryClient.Query(
            new QueryCommand<Input.Input>(
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

public sealed class SearchResultsComponent : ComponentBase
{
    public SearchResultsComponent(IDocumentQueryClientAccessor documentQueryClientAccessor) : base(documentQueryClientAccessor)
    {

    }
    internal override IQuerySelector Container => new ElementSelector("#results");

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
                    return new EstablishmentSearchResult(
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

public record EstablishmentSearchResult(
    string name, 
    string urn, 
    string typeOfEstablishment,
    string status,
    string phase);