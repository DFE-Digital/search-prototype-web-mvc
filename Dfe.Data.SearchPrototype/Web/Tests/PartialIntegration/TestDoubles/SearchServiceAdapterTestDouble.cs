using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.ServiceAdapters;
using Moq;

namespace Dfe.Data.SearchPrototype.Web.Tests.PartialIntegration.TestDoubles;

public static class SearchServiceAdapterTestDouble
{
    public static Mock<ISearchServiceAdapter> MockFor(Dfe.Data.SearchPrototype.SearchForEstablishments.Models.SearchResults stubSearchResults)
    {
        Mock<ISearchServiceAdapter> mockSearchServiceAdapter = DefaultMock();

        mockSearchServiceAdapter.Setup(adapter =>
            adapter.SearchAsync(It.IsAny<SearchServiceAdapterRequest>()))
                .ReturnsAsync(stubSearchResults);

        return mockSearchServiceAdapter;
    }

    public static Mock<ISearchServiceAdapter> DefaultMock() => new Mock<ISearchServiceAdapter>();
}
