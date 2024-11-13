using Azure.Search.Documents.Models;
using Azure;
using Moq;
using Dfe.Data.SearchPrototype.Infrastructure.DataTransferObjects;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles.Builder;

public sealed class SearchResponseBuilder
{
    private readonly IEstablishmentBuilder _establishmentBuilder;
    private readonly List<Establishment> _establishments = [];

    public SearchResponseBuilder(IEstablishmentBuilder establishmentBuilder)
    {
        ArgumentNullException.ThrowIfNull(establishmentBuilder);
        _establishmentBuilder = establishmentBuilder;
    }

    public SearchResponseBuilder AddEstablishment(Action<IEstablishmentBuilder> configureBuilder)
    {
        configureBuilder(_establishmentBuilder);
        _establishments.Add(
            _establishmentBuilder.Build());
        return this;
    }

    public SearchResponseBuilder ClearEstablishments()
    {
        _establishments.Clear();
        return this;
    }

    public Response<SearchResults<Establishment>> Build()
    {
        var responseMock = new Mock<Response>();

        IEnumerable<SearchResult<Establishment>> searchResults =
            _establishments.Select(
                (establishment) => SearchModelFactory.SearchResult(
                    document: establishment,
                    score: 0.9,
                    highlights: null));

        return Response.FromValue(
                SearchModelFactory.SearchResults(
                    searchResults,
                    searchResults.Count(),
                    facets: null,
                    null,
                    responseMock.Object),
                responseMock.Object);
    }
}
