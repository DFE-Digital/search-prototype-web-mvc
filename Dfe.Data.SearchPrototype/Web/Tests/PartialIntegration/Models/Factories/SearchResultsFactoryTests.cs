using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Web.Mappers;
using Dfe.Data.SearchPrototype.Web.Models.Factories;
using Dfe.Data.SearchPrototype.Web.Services;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;
using FluentAssertions;
using Moq;
using Xunit;
using Establishment = Dfe.Data.SearchPrototype.Web.Models.ViewModels.Establishment;
using SearchResults = Dfe.Data.SearchPrototype.Web.Models.ViewModels.SearchResults;

namespace Dfe.Data.SearchPrototype.Web.Tests.PartialIntegration.Models.Factories
{
    public sealed class SearchResultsFactoryTests
    {
        private ISearchResultsFactory _searchResultsFactory;
        private IMapper<EstablishmentResults?, List<Establishment>?> _establishmentResultsToEstablishmentsViewModelMapper;
        private IMapper<FacetsAndSelectedFacets, List<Web.Models.ViewModels.Facet>?> _facetsAndSelectedFacetsToFacetsViewModelMapper;
        private INameKeyToDisplayNameProvider displayNamesProviderMock = new Mock<INameKeyToDisplayNameProvider>().Object;
        public SearchResultsFactoryTests()
        {
            _establishmentResultsToEstablishmentsViewModelMapper =
                new EstablishmentResultsToEstablishmentsViewModelMapper();
            _facetsAndSelectedFacetsToFacetsViewModelMapper =
                new FacetsAndSelectedFacetsToFacetsViewModelMapper();
            _searchResultsFactory =
                new SearchResultsFactory(
                    _establishmentResultsToEstablishmentsViewModelMapper,
                    _facetsAndSelectedFacetsToFacetsViewModelMapper
                    );
        }

        [Fact]
        public void CreateViewModel_WithValidInput_ReturnsConfiguredSearchResults()
        {
            // act
            SearchResults? result =
                _searchResultsFactory.CreateViewModel(
                    establishmentResults: EstablishmentResultsTestDouble.Create(),
                    facetsAndSelectedFacets: FacetsAndSelectedFacetsTestDouble.Create());

            // assert
            result.Should().NotBeNull().And.BeOfType<SearchResults>();
            result.SearchItems.Should().HaveCountGreaterThanOrEqualTo(1).And.BeOfType<List<Establishment>>();
            result.SearchResultsCount.Should().BeGreaterThanOrEqualTo(1);
            result.HasResults.Should().BeTrue();
            result.Facets.Should().HaveCountGreaterThanOrEqualTo(1).And.BeOfType<List<Web.Models.ViewModels.Facet>>();
        }

        [Fact]
        public void CreateViewModel_WithNullEstablishmentResultsParam_ReturnsEmptySearchResults()
        {
            // act
            SearchResults? result =
                _searchResultsFactory.CreateViewModel(
                    establishmentResults: null!,
                    facetsAndSelectedFacets: FacetsAndSelectedFacetsTestDouble.Create());

            // assert
            result.Should().NotBeNull().And.BeOfType<SearchResults>();
            result.SearchItems.Should().BeNull();
            result.SearchResultsCount.Should().Be(0);
            result.HasResults.Should().BeFalse();
            result.Facets.Should().BeNull();
        }

        [Fact]
        public void CreateViewModel_WithNullFacetsAndSelectedFacetsParam_ReturnsSearchResultsWithNullFacets()
        {
            // act
            SearchResults? result =
                _searchResultsFactory.CreateViewModel(
                    establishmentResults: EstablishmentResultsTestDouble.Create(),
                    facetsAndSelectedFacets: null!);

            // assert
            result.Should().NotBeNull().And.BeOfType<SearchResults>();
            result.SearchItems.Should().HaveCountGreaterThanOrEqualTo(1).And.BeOfType<List<Establishment>>();
            result.SearchResultsCount.Should().BeGreaterThanOrEqualTo(1);
            result.HasResults.Should().BeTrue();
            result.Facets.Should().BeNull();
        }

        [Fact]
        public void CreateViewModel_WithFacetsAndSelectedFacetsParamNullFacets_ReturnsSearchResultsWithNullFacets()
        {
            // act
            SearchResults? result =
                _searchResultsFactory.CreateViewModel(
                    establishmentResults: EstablishmentResultsTestDouble.Create(),
                    facetsAndSelectedFacets: FacetsAndSelectedFacetsTestDouble.CreateWith(establishmentFacets: null!, selectedFacets: null!));

            // assert
            result.Should().NotBeNull().And.BeOfType<SearchResults>();
            result.SearchItems.Should().HaveCountGreaterThanOrEqualTo(1).And.BeOfType<List<Establishment>>();
            result.SearchResultsCount.Should().BeGreaterThanOrEqualTo(1);
            result.HasResults.Should().BeTrue();
            result.Facets.Should().BeNull();
        }
    }
}