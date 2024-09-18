using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.Web.Mappers;
using Dfe.Data.SearchPrototype.Web.ViewModels;
using Moq;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.TestDoubles
{
    public static class EstablishmentFacetsToFacetsViewModelMapperTestDouble
    {
        public static Mock<IMapper<EstablishmentFacetsMapperRequest, List<Facet>?>> MockFor(List<Facet>? viewModel)
        {
            Mock<IMapper<EstablishmentFacetsMapperRequest, List<Facet>?>> mockMapper = DefaultMock();

            mockMapper.Setup(mapper =>
                mapper.MapFrom(It.IsAny<EstablishmentFacetsMapperRequest>())).Returns(viewModel);

            return mockMapper;
        }

        public static Mock<IMapper<EstablishmentFacetsMapperRequest, List<Facet>?>> DefaultMock() =>
            new Mock<IMapper<EstablishmentFacetsMapperRequest, List<Facet>?>>();
    }
}
