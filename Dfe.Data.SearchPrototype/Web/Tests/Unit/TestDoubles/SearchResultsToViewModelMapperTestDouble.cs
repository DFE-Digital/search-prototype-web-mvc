using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments;
using Dfe.Data.SearchPrototype.Web.Models;
using Moq;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.TestDoubles
{
    public static class SearchResultsToViewModelMapperTestDouble
    {
        public static Mock<IMapper<SearchByKeywordResponse, SearchResultsViewModel>> MockFor(SearchResultsViewModel viewModel)
        {
            Mock<IMapper<SearchByKeywordResponse, SearchResultsViewModel>> mockMapper = DefaultMock();

            mockMapper.Setup(mapper =>
                mapper.MapFrom(It.IsAny<SearchByKeywordResponse>())).Returns(viewModel);

            return mockMapper;
        }

        public static Mock<IMapper<SearchByKeywordResponse, SearchResultsViewModel>> DefaultMock()
        {
            return new Mock<IMapper<SearchByKeywordResponse, SearchResultsViewModel>>();
        }
    }
}
