using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Web.Mappers;
using Dfe.Data.SearchPrototype.Web.Models.Factories;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;
using Dfe.Data.SearchPrototype.Web.Tests.Unit.TestDoubles;
using ViewModels = Dfe.Data.SearchPrototype.Web.Models.ViewModels;
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
            Mock<IMapper<EstablishmentResults?, List<ViewModels.Establishment>?>> mockEstablishmentResultsToEstablishmentsViewModelMapper =
                EstablishmentResultsToEstablishmentsViewModelMapperTestDouble.MockFor([]);

            Mock<IMapper<FacetsAndSelectedFacets, List<ViewModels.Facet>?>> mockEstablishmentFacetsToFacetsViewModelMapper =
                FacetsAndSelectedFacetsToFacetsViewModelMapperTestDouble.MockFor([]);

            SearchResultsFactory searchResultsFactory =
                new(mockEstablishmentResultsToEstablishmentsViewModelMapper.Object,
                    mockEstablishmentFacetsToFacetsViewModelMapper.Object);

            EstablishmentResults establishmentResults = EstablishmentResultsTestDouble.Create();
            FacetsAndSelectedFacets facetsAndSelectedFacets = FacetsAndSelectedFacetsTestDouble.Create();

            //act
            searchResultsFactory.CreateViewModel(establishmentResults, facetsAndSelectedFacets);

            // verify
            mockEstablishmentResultsToEstablishmentsViewModelMapper.Verify(mapper => mapper.MapFrom(establishmentResults), Times.Once());
            mockEstablishmentFacetsToFacetsViewModelMapper.Verify(mapper => mapper.MapFrom(facetsAndSelectedFacets), Times.Once());
        }

        [Fact]
        public void CreateViewModel_NullEstablishmentResultParam_CallsMappers()
        {
            // arrange
            Mock<IMapper<EstablishmentResults?, List<ViewModels.Establishment>?>> mockEstablishmentResultsToEstablishmentsViewModelMapper =
            EstablishmentResultsToEstablishmentsViewModelMapperTestDouble.MockFor([]);

            Mock<IMapper<FacetsAndSelectedFacets, List<ViewModels.Facet>?>> mockEstablishmentFacetsToFacetsViewModelMapper =
                FacetsAndSelectedFacetsToFacetsViewModelMapperTestDouble.MockFor([]);

            SearchResultsFactory searchResultsFactory =
                new(mockEstablishmentResultsToEstablishmentsViewModelMapper.Object,
                    mockEstablishmentFacetsToFacetsViewModelMapper.Object);

            EstablishmentResults establishmentResults = null!;
            FacetsAndSelectedFacets facetsAndSelectedFacets = FacetsAndSelectedFacetsTestDouble.Create();

            //act
            searchResultsFactory.CreateViewModel(establishmentResults, facetsAndSelectedFacets);

            // verify
            mockEstablishmentResultsToEstablishmentsViewModelMapper.Verify(mapper => mapper.MapFrom(It.IsAny<EstablishmentResults?>()), Times.Never());
            mockEstablishmentFacetsToFacetsViewModelMapper.Verify(mapper => mapper.MapFrom(facetsAndSelectedFacets), Times.Never());
        }

        [Fact]
        public void CreateViewModel_NullfacetsAndSelectedFacetsParam_CallsMappers()
        {
            // arrange
            Mock<IMapper<EstablishmentResults?, List<ViewModels.Establishment>?>> mockEstablishmentResultsToEstablishmentsViewModelMapper =
            EstablishmentResultsToEstablishmentsViewModelMapperTestDouble.MockFor([]);

            Mock<IMapper<FacetsAndSelectedFacets, List<ViewModels.Facet>?>> mockEstablishmentFacetsToFacetsViewModelMapper =
                FacetsAndSelectedFacetsToFacetsViewModelMapperTestDouble.MockFor([]);

            EstablishmentResults establishmentResults = EstablishmentResultsTestDouble.Create();
            FacetsAndSelectedFacets facetsAndSelectedFacets = null!;

            
            SearchResultsFactory searchResultsFactory =
                new(mockEstablishmentResultsToEstablishmentsViewModelMapper.Object,
                    mockEstablishmentFacetsToFacetsViewModelMapper.Object);
            //act
            searchResultsFactory.CreateViewModel(establishmentResults, facetsAndSelectedFacets);

            // verify
            mockEstablishmentResultsToEstablishmentsViewModelMapper.Verify(mapper => mapper.MapFrom(establishmentResults), Times.Once());
            mockEstablishmentFacetsToFacetsViewModelMapper.Verify(mapper => mapper.MapFrom(facetsAndSelectedFacets), Times.Once());
        }
    }
}