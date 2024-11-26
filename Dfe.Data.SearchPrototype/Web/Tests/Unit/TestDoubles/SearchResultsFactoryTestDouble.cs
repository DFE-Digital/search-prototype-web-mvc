using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Web.Mappers;
using Dfe.Data.SearchPrototype.Web.Models.Factories;
using Moq;
using ViewModels = Dfe.Data.SearchPrototype.Web.Models.ViewModels;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.TestDoubles
{
    public static class SearchResultsFactoryTestDouble
    {
        public static Mock<ISearchResultsFactory> MockFor(ViewModels.SearchResults viewModel)
        {
            Mock<ISearchResultsFactory> mockSearchResultsFactory = DefaultMock();

            mockSearchResultsFactory.Setup(mapper =>
                mapper.CreateViewModel(
                    It.IsAny<EstablishmentResults>(),
                    It.IsAny<FacetsAndSelectedFacets>(),
                    It.IsAny<int>(),
                    It.IsAny<int>())).Returns(viewModel);

            return mockSearchResultsFactory;
        }

        public static Mock<ISearchResultsFactory> DefaultMock() => new();
    }
}
