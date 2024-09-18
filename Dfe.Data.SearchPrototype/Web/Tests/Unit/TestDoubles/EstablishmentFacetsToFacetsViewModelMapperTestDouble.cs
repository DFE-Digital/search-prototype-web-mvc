using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Web.ViewModels;
using Moq;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.TestDoubles
{
    public static class EstablishmentFacetsToFacetsViewModelMapperTestDouble
    {
        public static Mock<IMapper<(EstablishmentFacets?, Dictionary<string, List<string>>?), List<Facet>?>> MockFor(List<Facet>? viewModel)
        {
            Mock<IMapper<(EstablishmentFacets?, Dictionary<string, List<string>>?), List<Facet>?>> mockMapper = DefaultMock();

            mockMapper.Setup(mapper =>
                mapper.MapFrom(It.IsAny<(EstablishmentFacets?, Dictionary<string, List<string>>?)>())).Returns(viewModel);

            return mockMapper;
        }

        public static Mock<IMapper<(EstablishmentFacets?, Dictionary<string, List<string>>?), List<Facet>?>> DefaultMock() =>
            new Mock<IMapper<(EstablishmentFacets?, Dictionary<string, List<string>>?), List<Facet>?>>();
    }
}
