using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Moq;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.TestDoubles
{
    public static class EstablishmentResultsToEstablishmentsViewModelMapperTestDouble
    {
        public static Mock<IMapper<EstablishmentResults?, List<Web.Models.ViewModels.Establishment>?>> MockFor(List<Web.Models.ViewModels.Establishment>? viewModel)
        {
            Mock<IMapper<EstablishmentResults?, List<Web.Models.ViewModels.Establishment>?>> mockMapper = DefaultMock();

            mockMapper.Setup(mapper =>
                mapper.MapFrom(It.IsAny<EstablishmentResults?>())).Returns(viewModel);

            return mockMapper;
        }

        public static Mock<IMapper<EstablishmentResults?, List<Web.Models.ViewModels.Establishment>?>> DefaultMock() => new();
    }
}
