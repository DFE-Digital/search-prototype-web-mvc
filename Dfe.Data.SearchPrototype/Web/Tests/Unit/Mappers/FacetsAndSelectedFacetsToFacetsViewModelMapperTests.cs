﻿using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Web.Mappers;
using Dfe.Data.SearchPrototype.Web.Models.ViewModels;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;
using Xunit;
using SearchResults = Dfe.Data.SearchPrototype.Web.Models.ViewModels.SearchResults;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.Mappers
{
    public sealed class FacetsAndSelectedFacetsToFacetsViewModelMapperTests
    {
        private readonly IMapper<FacetsAndSelectedFacets, List<Facet>?> _facetsAndSelectedFacetsToFacetsViewModelMapper
            = new FacetsAndSelectedFacetsToFacetsViewModelMapper();

        [Fact]
        public void Mapper_WithFacetResults_ReturnsFacetsInViewModel()
        {
            // arrange.
            var response = SearchByKeywordResponseTestDouble.Create();

            // act.
            SearchResults viewModelResults = new()
            {
                Facets =
                    _facetsAndSelectedFacetsToFacetsViewModelMapper.MapFrom(
                        new FacetsAndSelectedFacets(response.EstablishmentFacetResults))
            };

            // assert
            foreach (var facetedField in response.EstablishmentFacetResults!.Facets!) // for each FacetedField (e.g. Phase of education)
            {
                var equivalentFacetedField = viewModelResults.Facets!.First(facet => facet.Name == facetedField.Name);
                Assert.NotNull(equivalentFacetedField); // the name has been mapped correctly

                foreach (var expectedFacet in facetedField.Results) // for each facet (value) within this faceted field (e.g. 'primary')
                {
                    var equivalentFacet = equivalentFacetedField.Values.First(facetValue => facetValue.Value == expectedFacet.Value); // find the equivalent facet in the mapped response
                    Assert.NotNull(equivalentFacet);
                    Assert.Equal(expectedFacet.Count, equivalentFacet.Count);
                }
            }
        }

        [Fact]
        public void Mapper_NullFacetsResults_ReturnNullFacetsInViewModel()
        {
            // arrange.
            SearchByKeywordResponse response = SearchByKeywordResponseTestDouble.CreateWithNoResults();

            // act.
            SearchResults viewModelResults = new()
            {
                Facets =
                    _facetsAndSelectedFacetsToFacetsViewModelMapper.MapFrom(
                        new FacetsAndSelectedFacets(response.EstablishmentFacetResults))
            };

            // assert.
            Assert.Null(viewModelResults.Facets);
        }

        [Fact]
        public void Mapper_EmptyFacetsResults_ReturnEmptyFacetsInViewModel()
        {
            // arrange.
            SearchByKeywordResponse response = SearchByKeywordResponseTestDouble.CreateWithEmptyList();

            // act.
            SearchResults viewModelResults = new()
            {
                Facets =
                    _facetsAndSelectedFacetsToFacetsViewModelMapper.MapFrom(
                        new FacetsAndSelectedFacets(response.EstablishmentFacetResults))
            };

            // assert.
            Assert.Empty(viewModelResults.Facets!);
        }

        [Fact]
        public void Mapper_WithFacetResults_ReturnsSelectedFacetsTrue()
        {
            // arrange.
            SearchByKeywordResponse response = new(SearchResponseStatus.Success)
            {
                EstablishmentResults = new EstablishmentResults(),
                EstablishmentFacetResults = new EstablishmentFacets(
                    new List<EstablishmentFacet>()
                    {
                        EstablishmentFacetTestDouble.CreateWith("PHASEOFEDUCATION", "Primary", 2),
                        EstablishmentFacetTestDouble.CreateWith("ESTABLISHMENTSTATUS", "Open", 21),
                    }),
                TotalNumberOfEstablishments = 10
            };
   
            Dictionary<string, List<string>>? selectedFacets = new() {
                {"PHASEOFEDUCATION",new List<string>{ } },
                {"ESTABLISHMENTSTATUS", new List<string> {"Open", "Closed"}}
            };

            // act.
            SearchResults viewModelResults = new()
            {
                Facets =
                    _facetsAndSelectedFacetsToFacetsViewModelMapper.MapFrom(
                        new FacetsAndSelectedFacets(response.EstablishmentFacetResults, selectedFacets))
            };

            // assert
            var facetValues = viewModelResults.Facets!.Find(f => f.Name == "PHASEOFEDUCATION")!.Values;
            var facetValue = Assert.Single(facetValues);
            Assert.False(facetValue.IsSelected);
            Assert.Equal("Primary", facetValue.Value);

            var facetValues2 = viewModelResults.Facets.Find(f => f.Name == "ESTABLISHMENTSTATUS")!.Values;
            Assert.True(facetValues2.Single(facetValue => facetValue.Value == "Open").IsSelected);
        }
    }
}
