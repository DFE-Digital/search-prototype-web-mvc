using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.Infrastructure.Tests.TestDoubles.Shared;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Web.Mappers;
using Dfe.Data.SearchPrototype.Web.Models.Factories;
using Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared;
using Dfe.Data.SearchPrototype.Web.Options;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;
using Establishment = Dfe.Data.SearchPrototype.Web.Models.ViewModels.Establishment;
using SearchResults = Dfe.Data.SearchPrototype.Web.Models.ViewModels.SearchResults;

namespace Dfe.Data.SearchPrototype.Web.Tests.PartialIntegration.Models.Factories
{
    public sealed class SearchResultsFactoryTests
    {
        [Fact]
        public void CreateViewModel_WithValidInput_ReturnsConfiguredSearchResults()
        {
            // arrange
            IMapper<EstablishmentResults?, List<Establishment>?> establishmentResultsToEstablishmentsViewModelMapper =
                new EstablishmentResultsToEstablishmentsViewModelMapper();
            IMapper<FacetsAndSelectedFacets, List<SearchPrototype.Web.Models.ViewModels.Facet>?> _facetsAndSelectedFacetsToFacetsViewModelMapper =
                new FacetsAndSelectedFacetsToFacetsViewModelMapper();
            IOptions<PaginationOptions> paginationOptions =
                IOptionsTestDouble.IOptionsMockFor(new PaginationOptions() { RecordsPerPage = 10 });
            IMapper<(int, int), Pagination> _paginationMapper =
                new PaginationResultsToPaginationViewModelMapper(new ScrollablePager(), paginationOptions);

            ISearchResultsFactory searchResultsFactory =
                new SearchResultsFactory(
                    establishmentResultsToEstablishmentsViewModelMapper,
                    _facetsAndSelectedFacetsToFacetsViewModelMapper,
                    _paginationMapper);

            // act
            SearchResults? result =
                searchResultsFactory.CreateViewModel(
                    establishmentResults: EstablishmentResultsTestDouble.Create(),
                    facetsAndSelectedFacets: FacetsAndSelectedFacetsTestDouble.Create(),
                    totalNumberOfEstablishments: 10,
                    currentPageNumber: 1);

            // assert
            result.Should().NotBeNull().And.BeOfType<SearchResults>();
            result.SearchItems.Should().HaveCountGreaterThanOrEqualTo(1).And.BeOfType<List<Establishment>>();
            result.SearchResultsCount.Should().BeGreaterThanOrEqualTo(1);
            result.HasResults.Should().BeTrue();
            result.Facets.Should().HaveCountGreaterThanOrEqualTo(1).And.BeOfType<List<SearchPrototype.Web.Models.ViewModels.Facet>>();
        }

        [Fact]
        public void CreateViewModel_WithNullEstablishmentResultsParam_ReturnsEmptySearchResults()
        {
            // arrange
            IMapper<EstablishmentResults?, List<Establishment>?> establishmentResultsToEstablishmentsViewModelMapper =
                new EstablishmentResultsToEstablishmentsViewModelMapper();
            IMapper<FacetsAndSelectedFacets, List<SearchPrototype.Web.Models.ViewModels.Facet>?> _facetsAndSelectedFacetsToFacetsViewModelMapper =
                new FacetsAndSelectedFacetsToFacetsViewModelMapper();
            IOptions<PaginationOptions> paginationOptions =
                IOptionsTestDouble.IOptionsMockFor(new PaginationOptions() { RecordsPerPage = 10 });
            IMapper<(int, int), Pagination> _paginationMapper =
                new PaginationResultsToPaginationViewModelMapper(new ScrollablePager(), paginationOptions);

            ISearchResultsFactory searchResultsFactory =
                new SearchResultsFactory(
                    establishmentResultsToEstablishmentsViewModelMapper,
                    _facetsAndSelectedFacetsToFacetsViewModelMapper,
                    _paginationMapper);

            // act
            SearchResults? result =
                searchResultsFactory.CreateViewModel(
                    establishmentResults: null!,
                    facetsAndSelectedFacets: FacetsAndSelectedFacetsTestDouble.Create(),
                    totalNumberOfEstablishments: 10,
                    currentPageNumber: 1);

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
            // arrange
            IMapper<EstablishmentResults?, List<Establishment>?> establishmentResultsToEstablishmentsViewModelMapper =
                new EstablishmentResultsToEstablishmentsViewModelMapper();
            IMapper<FacetsAndSelectedFacets, List<SearchPrototype.Web.Models.ViewModels.Facet>?> _facetsAndSelectedFacetsToFacetsViewModelMapper =
                new FacetsAndSelectedFacetsToFacetsViewModelMapper();
            IOptions<PaginationOptions> paginationOptions =
                IOptionsTestDouble.IOptionsMockFor(new PaginationOptions() { RecordsPerPage = 10 });
            IMapper<(int, int), Pagination> _paginationMapper =
                new PaginationResultsToPaginationViewModelMapper(new ScrollablePager(), paginationOptions);

            ISearchResultsFactory searchResultsFactory =
                new SearchResultsFactory(
                    establishmentResultsToEstablishmentsViewModelMapper,
                    _facetsAndSelectedFacetsToFacetsViewModelMapper,
                    _paginationMapper);

            // act
            SearchResults? result =
                searchResultsFactory.CreateViewModel(
                    establishmentResults: EstablishmentResultsTestDouble.Create(),
                    facetsAndSelectedFacets: null!,
                    totalNumberOfEstablishments: 10,
                    currentPageNumber: 1);

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
            // arrange
            IMapper<EstablishmentResults?, List<Establishment>?> establishmentResultsToEstablishmentsViewModelMapper =
                new EstablishmentResultsToEstablishmentsViewModelMapper();
            IMapper<FacetsAndSelectedFacets, List<SearchPrototype.Web.Models.ViewModels.Facet>?> _facetsAndSelectedFacetsToFacetsViewModelMapper =
                new FacetsAndSelectedFacetsToFacetsViewModelMapper();
            IOptions<PaginationOptions> paginationOptions =
                IOptionsTestDouble.IOptionsMockFor(new PaginationOptions() { RecordsPerPage = 10 });
            IMapper<(int, int), Pagination> _paginationMapper =
                new PaginationResultsToPaginationViewModelMapper(new ScrollablePager(), paginationOptions);

            ISearchResultsFactory searchResultsFactory =
                new SearchResultsFactory(
                    establishmentResultsToEstablishmentsViewModelMapper,
                    _facetsAndSelectedFacetsToFacetsViewModelMapper,
                    _paginationMapper);

            // act
            SearchResults? result =
                searchResultsFactory.CreateViewModel(
                    establishmentResults: EstablishmentResultsTestDouble.Create(),
                    facetsAndSelectedFacets: FacetsAndSelectedFacetsTestDouble.CreateWith(establishmentFacets: null!, selectedFacets: null!),
                    totalNumberOfEstablishments: 10,
                    currentPageNumber: 1);

            // assert
            result.Should().NotBeNull().And.BeOfType<SearchResults>();
            result.SearchItems.Should().HaveCountGreaterThanOrEqualTo(1).And.BeOfType<List<Establishment>>();
            result.SearchResultsCount.Should().BeGreaterThanOrEqualTo(1);
            result.HasResults.Should().BeTrue();
            result.Facets.Should().BeNull();
        }
    }
}