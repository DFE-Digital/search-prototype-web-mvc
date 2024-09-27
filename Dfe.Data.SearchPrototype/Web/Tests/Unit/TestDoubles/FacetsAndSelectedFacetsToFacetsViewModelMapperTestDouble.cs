using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.Web.Mappers;
using Dfe.Data.SearchPrototype.Web.Models.ViewModels;
using Moq;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.TestDoubles
{
    public static class FacetsAndSelectedFacetsToFacetsViewModelMapperTestDouble
    {
        public static Mock<IMapper<FacetsAndSelectedFacets, List<Facet>?>> MockFor(List<Facet>? viewModel)
        {
            Mock<IMapper<FacetsAndSelectedFacets, List<Facet>?>> mockMapper = DefaultMock();

            mockMapper.Setup(mapper =>
                mapper.MapFrom(It.IsAny<FacetsAndSelectedFacets>())).Returns(viewModel);

            return mockMapper;
        }

        public static Mock<IMapper<FacetsAndSelectedFacets, List<Facet>?>> DefaultMock() => new();
    }
}
