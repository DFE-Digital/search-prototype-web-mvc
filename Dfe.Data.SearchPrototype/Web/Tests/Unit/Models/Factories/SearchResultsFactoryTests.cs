using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Web.Mappers;
using Dfe.Data.SearchPrototype.Web.Models.Factories;
using Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared;
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
            Mock<IMapper<EstablishmentResults?, List<SearchPrototype.Web.Models.ViewModels.Establishment>?>> mockEstablishmentResultsToEstablishmentsViewModelMapper =
                EstablishmentResultsToEstablishmentsViewModelMapperTestDouble.MockFor([]);

            Mock<IMapper<FacetsAndSelectedFacets, List<SearchPrototype.Web.Models.ViewModels.Facet>?>> mockEstablishmentFacetsToFacetsViewModelMapper =
                FacetsAndSelectedFacetsToFacetsViewModelMapperTestDouble.MockFor([]);

            Mock<IMapper<(int, int), Pagination>> mockPaginationMapper =
                PaginationMapperTestDouble.MockFor(new Pagination(new ScrollablePager()));

            SearchResultsFactory searchResultsFactory =
                new(mockEstablishmentResultsToEstablishmentsViewModelMapper.Object,
                    mockEstablishmentFacetsToFacetsViewModelMapper.Object,
                    mockPaginationMapper.Object);

            EstablishmentResults establishmentResults = EstablishmentResultsTestDouble.Create();
            FacetsAndSelectedFacets facetsAndSelectedFacets = FacetsAndSelectedFacetsTestDouble.Create();

            //act
            searchResultsFactory.CreateViewModel(
                establishmentResults, facetsAndSelectedFacets, totalNumberOfEstablishments: 10, currentPageNumber: 1);

            // verify
            mockEstablishmentResultsToEstablishmentsViewModelMapper.Verify(mapper => mapper.MapFrom(establishmentResults), Times.Once());
            mockEstablishmentFacetsToFacetsViewModelMapper.Verify(mapper => mapper.MapFrom(facetsAndSelectedFacets), Times.Once());
            mockPaginationMapper.Verify(mapper => mapper.MapFrom(new (1, 10)), Times.Once());
        }

        [Fact]
        public void CreateViewModel_NullEstablishmentResultParam_NoCallsToMappers()
        {
            // arrange
            Mock<IMapper<EstablishmentResults?, List<SearchPrototype.Web.Models.ViewModels.Establishment>?>> mockEstablishmentResultsToEstablishmentsViewModelMapper =
            EstablishmentResultsToEstablishmentsViewModelMapperTestDouble.MockFor([]);

            Mock<IMapper<FacetsAndSelectedFacets, List<SearchPrototype.Web.Models.ViewModels.Facet>?>> mockEstablishmentFacetsToFacetsViewModelMapper =
                FacetsAndSelectedFacetsToFacetsViewModelMapperTestDouble.MockFor([]);

            Mock<IMapper<(int, int), Pagination>> mockPaginationMapper =
                PaginationMapperTestDouble.MockFor(new Pagination(new ScrollablePager()));

            SearchResultsFactory searchResultsFactory =
                new(mockEstablishmentResultsToEstablishmentsViewModelMapper.Object,
                    mockEstablishmentFacetsToFacetsViewModelMapper.Object,
                    mockPaginationMapper.Object);

            EstablishmentResults establishmentResults = null!;
            FacetsAndSelectedFacets facetsAndSelectedFacets = FacetsAndSelectedFacetsTestDouble.Create();

            //act
            searchResultsFactory.CreateViewModel(establishmentResults, facetsAndSelectedFacets, totalNumberOfEstablishments: 10, currentPageNumber:1);

            // verify
            mockEstablishmentResultsToEstablishmentsViewModelMapper.Verify(mapper => mapper.MapFrom(It.IsAny<EstablishmentResults?>()), Times.Never());
            mockEstablishmentFacetsToFacetsViewModelMapper.Verify(mapper => mapper.MapFrom(facetsAndSelectedFacets), Times.Never());
            mockPaginationMapper.Verify(mapper => mapper.MapFrom(new(1, 10)), Times.Never());
        }

        [Fact]
        public void CreateViewModel_NullfacetsAndSelectedFacetsParam_CallsMappers()
        {
            // arrange
            Mock<IMapper<EstablishmentResults?, List<SearchPrototype.Web.Models.ViewModels.Establishment>?>> mockEstablishmentResultsToEstablishmentsViewModelMapper =
            EstablishmentResultsToEstablishmentsViewModelMapperTestDouble.MockFor([]);

            Mock<IMapper<FacetsAndSelectedFacets, List<SearchPrototype.Web.Models.ViewModels.Facet>?>> mockEstablishmentFacetsToFacetsViewModelMapper =
                FacetsAndSelectedFacetsToFacetsViewModelMapperTestDouble.MockFor([]);

            Mock<IMapper<(int, int), Pagination>> mockPaginationMapper =
                PaginationMapperTestDouble.MockFor(new Pagination(new ScrollablePager()));

            EstablishmentResults establishmentResults = EstablishmentResultsTestDouble.Create();
            FacetsAndSelectedFacets facetsAndSelectedFacets = null!;

            
            SearchResultsFactory searchResultsFactory =
                new(mockEstablishmentResultsToEstablishmentsViewModelMapper.Object,
                    mockEstablishmentFacetsToFacetsViewModelMapper.Object,
                    mockPaginationMapper.Object);
            //act
            searchResultsFactory.CreateViewModel(
                establishmentResults, facetsAndSelectedFacets, totalNumberOfEstablishments: 10, currentPageNumber: 1);

            // verify
            mockEstablishmentResultsToEstablishmentsViewModelMapper.Verify(mapper => mapper.MapFrom(establishmentResults), Times.Once());
            mockEstablishmentFacetsToFacetsViewModelMapper.Verify(mapper => mapper.MapFrom(facetsAndSelectedFacets), Times.Once());
            mockPaginationMapper.Verify(mapper => mapper.MapFrom(new(1, 10)), Times.Once());
        }
    }
}