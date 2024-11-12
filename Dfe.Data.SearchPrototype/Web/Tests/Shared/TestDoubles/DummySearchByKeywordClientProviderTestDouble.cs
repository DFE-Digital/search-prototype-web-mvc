using Azure.Search.Documents.Models;
using Azure.Search.Documents;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers;
using Moq;
using Azure;
using Dfe.Data.SearchPrototype.Infrastructure.DataTransferObjects;

namespace DfE.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;

public class DummySearchByKeywordClientProviderTestDouble : ISearchByKeywordClientProvider
{
    private readonly SearchResponseBuilder _builder;

    public DummySearchByKeywordClientProviderTestDouble(SearchResponseBuilder builder)
    {
        _builder = builder;
    }

    public Task<SearchClient> InvokeSearchClientAsync(string indexName)
    {
        var clientMock = new Mock<SearchClient>(() 
            => new(
                new Uri("https://localhost"), indexName, new AzureKeyCredential("key")));

        clientMock.SetupGet(x => x.IndexName).Returns(indexName);

        clientMock.Setup(x => x.SearchAsync<Establishment>(
                It.IsAny<string>(),
                It.IsAny<SearchOptions>(), // TODO configure this?
                It.IsAny<CancellationToken>()
            )).Returns(
                Task.FromResult(
                    _builder.Build()));

        return Task.FromResult(clientMock.Object);
    }
}


public sealed class SearchResponseBuilder
{
    private readonly List<Establishment> _establishments  = [];
    
    public SearchResponseBuilder AddEstablishment(Establishment establishment)
    {
        ArgumentNullException.ThrowIfNull(establishment);
        _establishments.Add(establishment);
        return this;
    }

    public SearchResponseBuilder ClearEstablishments()
    {
        _establishments.Clear();
        return this;
    }

    // TODO public SearchResponseBuilder AddFacet()

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
