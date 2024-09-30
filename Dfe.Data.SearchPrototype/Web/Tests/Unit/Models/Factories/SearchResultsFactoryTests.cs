using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Web.Mappers;
using Dfe.Data.SearchPrototype.Web.Models.Factories;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;
using Dfe.Data.SearchPrototype.Web.Tests.Unit.TestDoubles;
using Moq;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.Models.Factories
{
    public sealed class SearchResultsFactoryTests
    {
        [Fact]
        public void CreateViewModel_ValidInput_CallsMappers()
        {
            // arrange
            Mock<IMapper<EstablishmentResults?, List<Web.Models.ViewModels.Establishment>?>> mockEstablishmentResultsToEstablishmentsViewModelMapper =
                EstablishmentResultsToEstablishmentsViewModelMapperTestDouble.MockFor([]);

            Mock<IMapper<FacetsAndSelectedFacets, List<Web.Models.ViewModels.Facet>?>> mockEstablishmentFacetsToFacetsViewModelMapper =
                FacetsAndSelectedFacetsToFacetsViewModelMapperTestDouble.MockFor([]);

            //act
            SearchResultsFactory searchResultsFactory =
                new(mockEstablishmentResultsToEstablishmentsViewModelMapper.Object,
                    mockEstablishmentFacetsToFacetsViewModelMapper.Object);

            // assert/verify
            searchResultsFactory.CreateViewModel(EstablishmentResultsTestDouble.Create(), FacetsAndSelectedFacetsTestDouble.Create());

            mockEstablishmentResultsToEstablishmentsViewModelMapper.Verify(mapper => mapper.MapFrom(It.IsAny<EstablishmentResults?>()), Times.Once());
            mockEstablishmentFacetsToFacetsViewModelMapper.Verify(mapper => mapper.MapFrom(It.IsAny<FacetsAndSelectedFacets>()), Times.Once());
        }

        [Fact]
        public void CreateViewModel_NullEstablishmentResultParam_CallsMappers()
        {
            // arrange
            Mock<IMapper<EstablishmentResults?, List<Web.Models.ViewModels.Establishment>?>> mockEstablishmentResultsToEstablishmentsViewModelMapper =
            EstablishmentResultsToEstablishmentsViewModelMapperTestDouble.MockFor([]);

            Mock<IMapper<FacetsAndSelectedFacets, List<Web.Models.ViewModels.Facet>?>> mockEstablishmentFacetsToFacetsViewModelMapper =
                FacetsAndSelectedFacetsToFacetsViewModelMapperTestDouble.MockFor([]);

            //act
            SearchResultsFactory searchResultsFactory =
                new(mockEstablishmentResultsToEstablishmentsViewModelMapper.Object,
                    mockEstablishmentFacetsToFacetsViewModelMapper.Object);

            // assert/verify
            searchResultsFactory.CreateViewModel(establishmentResults: null!, FacetsAndSelectedFacetsTestDouble.Create());

            mockEstablishmentResultsToEstablishmentsViewModelMapper.Verify(mapper => mapper.MapFrom(It.IsAny<EstablishmentResults?>()), Times.Never());
            mockEstablishmentFacetsToFacetsViewModelMapper.Verify(mapper => mapper.MapFrom(It.IsAny<FacetsAndSelectedFacets>()), Times.Never());
        }

        [Fact]
        public void CreateViewModel_NullfacetsAndSelectedFacetsParam_CallsMappers()
        {
            // arrange
            Mock<IMapper<EstablishmentResults?, List<Web.Models.ViewModels.Establishment>?>> mockEstablishmentResultsToEstablishmentsViewModelMapper =
            EstablishmentResultsToEstablishmentsViewModelMapperTestDouble.MockFor([]);

            Mock<IMapper<FacetsAndSelectedFacets, List<Web.Models.ViewModels.Facet>?>> mockEstablishmentFacetsToFacetsViewModelMapper =
                FacetsAndSelectedFacetsToFacetsViewModelMapperTestDouble.MockFor([]);

            //act
            SearchResultsFactory searchResultsFactory =
                new(mockEstablishmentResultsToEstablishmentsViewModelMapper.Object,
                    mockEstablishmentFacetsToFacetsViewModelMapper.Object);

            // assert/verify
            searchResultsFactory.CreateViewModel(establishmentResults: EstablishmentResultsTestDouble.Create(), facetsAndSelectedFacets: null!);

            mockEstablishmentResultsToEstablishmentsViewModelMapper.Verify(mapper => mapper.MapFrom(It.IsAny<EstablishmentResults?>()), Times.Once());
            mockEstablishmentFacetsToFacetsViewModelMapper.Verify(mapper => mapper.MapFrom(It.IsAny<FacetsAndSelectedFacets>()), Times.Once());
        }
    }
}