using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Web.Mappers;
using Dfe.Data.SearchPrototype.Web.Models.Factories;
using Dfe.Data.SearchPrototype.Web.Services;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;
using Dfe.Data.SearchPrototype.Web.Tests.Unit.TestDoubles;
using Moq;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.Models.Factories
{
    public sealed class SearchResultsFactoryTests
    {
        private Mock<IMapper<EstablishmentResults?, List<Web.Models.ViewModels.Establishment>?>> mockEstablishmentResultsToEstablishmentsViewModelMapper;
        private Mock<IMapper<FacetsAndSelectedFacets, List<Web.Models.ViewModels.Facet>?>> mockEstablishmentFacetsToFacetsViewModelMapper;
        private SearchResultsFactory searchResultsFactory;
        private INameKeyToDisplayNameProvider _facetNameToDisplayNameProvider;

        public SearchResultsFactoryTests()
        {
            _facetNameToDisplayNameProvider = new Mock<INameKeyToDisplayNameProvider>().Object;

            mockEstablishmentResultsToEstablishmentsViewModelMapper =
                EstablishmentResultsToEstablishmentsViewModelMapperTestDouble.MockFor([]);

            mockEstablishmentFacetsToFacetsViewModelMapper =
                FacetsAndSelectedFacetsToFacetsViewModelMapperTestDouble.MockFor([]);

            searchResultsFactory =
                new(mockEstablishmentResultsToEstablishmentsViewModelMapper.Object,
                    mockEstablishmentFacetsToFacetsViewModelMapper.Object,
                    _facetNameToDisplayNameProvider);
        }

        [Fact]
        public void CreateViewModel_ValidInput_CallsMappers()
        {
            // arrange
            //Mock<IMapper<EstablishmentResults?, List<Web.Models.ViewModels.Establishment>?>> mockEstablishmentResultsToEstablishmentsViewModelMapper =
            //    EstablishmentResultsToEstablishmentsViewModelMapperTestDouble.MockFor([]);

            //Mock<IMapper<FacetsAndSelectedFacets, List<Web.Models.ViewModels.Facet>?>> mockEstablishmentFacetsToFacetsViewModelMapper =
            //    FacetsAndSelectedFacetsToFacetsViewModelMapperTestDouble.MockFor([]);

            //SearchResultsFactory searchResultsFactory =
            //    new(mockEstablishmentResultsToEstablishmentsViewModelMapper.Object,
            //        mockEstablishmentFacetsToFacetsViewModelMapper.Object);

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
            //Mock<IMapper<EstablishmentResults?, List<Web.Models.ViewModels.Establishment>?>> mockEstablishmentResultsToEstablishmentsViewModelMapper =
            //EstablishmentResultsToEstablishmentsViewModelMapperTestDouble.MockFor([]);

            //Mock<IMapper<FacetsAndSelectedFacets, List<Web.Models.ViewModels.Facet>?>> mockEstablishmentFacetsToFacetsViewModelMapper =
            //    FacetsAndSelectedFacetsToFacetsViewModelMapperTestDouble.MockFor([]);

            //SearchResultsFactory searchResultsFactory =
            //    new(mockEstablishmentResultsToEstablishmentsViewModelMapper.Object,
            //        mockEstablishmentFacetsToFacetsViewModelMapper.Object);

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
            //Mock<IMapper<EstablishmentResults?, List<Web.Models.ViewModels.Establishment>?>> mockEstablishmentResultsToEstablishmentsViewModelMapper =
            //EstablishmentResultsToEstablishmentsViewModelMapperTestDouble.MockFor([]);

            //Mock<IMapper<FacetsAndSelectedFacets, List<Web.Models.ViewModels.Facet>?>> mockEstablishmentFacetsToFacetsViewModelMapper =
            //    FacetsAndSelectedFacetsToFacetsViewModelMapperTestDouble.MockFor([]);
            
            //SearchResultsFactory searchResultsFactory =
            //    new(mockEstablishmentResultsToEstablishmentsViewModelMapper.Object,
            //        mockEstablishmentFacetsToFacetsViewModelMapper.Object);

            EstablishmentResults establishmentResults = EstablishmentResultsTestDouble.Create();
            FacetsAndSelectedFacets facetsAndSelectedFacets = null!;

            //act
            searchResultsFactory.CreateViewModel(establishmentResults, facetsAndSelectedFacets);

            // verify
            mockEstablishmentResultsToEstablishmentsViewModelMapper.Verify(mapper => mapper.MapFrom(establishmentResults), Times.Once());
            mockEstablishmentFacetsToFacetsViewModelMapper.Verify(mapper => mapper.MapFrom(facetsAndSelectedFacets), Times.Once());
        }
    }
}