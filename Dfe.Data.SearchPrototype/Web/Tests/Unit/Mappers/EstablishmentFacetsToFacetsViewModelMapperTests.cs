using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.Web.Mappers;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;
using Dfe.Data.SearchPrototype.Web.ViewModels;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.Mappers
{
    public sealed class EstablishmentFacetsToFacetsViewModelMapperTests
    {
        private readonly IMapper<EstablishmentFacetsMapperRequest, List<Facet>?> _establishmentFacetsToFacetsViewModelMapper
            = new EstablishmentFacetsToFacetsViewModelMapper();

        [Fact]
        public void Mapper_WithFacetResults_ReturnsFacetsInViewModel()
        {
            // arrange.
            var response = SearchByKeywordResponseTestDouble.Create();

            // act.
            SearchResults viewModelResults = new(){
                Facets =
                    _establishmentFacetsToFacetsViewModelMapper.MapFrom(
                        new EstablishmentFacetsMapperRequest(response.EstablishmentFacetResults))
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
    }
}
