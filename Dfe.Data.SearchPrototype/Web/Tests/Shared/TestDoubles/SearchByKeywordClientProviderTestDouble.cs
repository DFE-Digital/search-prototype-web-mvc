using Azure;
using Azure.Search.Documents;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers;
using Dfe.Data.SearchPrototype.Infrastructure.DataTransferObjects;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles.Builder;
using Moq;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;


public class SearchByKeywordClientProviderTestDouble : ISearchByKeywordClientProvider
{
    private readonly SearchResponseBuilder _builder;

    public SearchByKeywordClientProviderTestDouble(SearchResponseBuilder builder)
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
