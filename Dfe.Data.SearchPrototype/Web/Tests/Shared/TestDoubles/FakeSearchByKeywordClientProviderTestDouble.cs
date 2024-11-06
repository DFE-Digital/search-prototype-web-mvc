using Azure.Search.Documents.Models;
using Azure.Search.Documents;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers;
using Moq;
using Azure;
using Dfe.Data.SearchPrototype.Infrastructure.DataTransferObjects;

namespace DfE.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;

public class FakeSearchByKeywordClientProviderTestDouble : ISearchByKeywordClientProvider
{
    public Task<SearchClient> InvokeSearchClientAsync(string indexName)
    {
        var responseMock = new Mock<Response>();

        var clientMock = new Mock<SearchClient>(() => new SearchClient(new Uri("https://localhost"), "establishments", new AzureKeyCredential("key")));

        Establishment myMock = new()
        {
            TYPEOFESTABLISHMENTNAME = "Blah",
            ESTABLISHMENTNAME = "Blah",
            id = "100000",
            PHASEOFEDUCATION = "Blah",
            ESTABLISHMENTSTATUSNAME = "Something"

        };
        clientMock.SetupGet(x => x.IndexName).Returns("establishments");
        clientMock.Setup(x => x.SearchAsync<Establishment>(
                It.IsAny<string>(),
                It.IsAny<SearchOptions>(),
                It.IsAny<CancellationToken>()
            ))
            .Returns(
                Task.FromResult(
                    Response.FromValue(
                        SearchModelFactory.SearchResults(new[]
                            {
                                    SearchModelFactory.SearchResult(myMock, 0.9, null),
                            //SearchModelFactory.SearchResult(new Hotel("2", "Two"), 0.8, null),
                            },
                            totalCount: 1,
                            null,
                            null,
                            responseMock.Object),
                        responseMock.Object)));

        return Task.FromResult(clientMock.Object);
    }
}
