using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments;
using Dfe.Data.SearchPrototype.Web.Mappers;
using Dfe.Data.SearchPrototype.Web.Models;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.Mappers;

public class SearchByKeywordResponseToViewModelMapperTests
{
    private readonly IMapper<SearchByKeywordResponse, SearchResultsViewModel> _serviceModelToViewModelMapper
        = new SearchByKeywordResponseToViewModelMapper();

    [Fact]
    public void Mapper_WithEstablishmentResults_ReturnsEstablishmentResultsInViewModel()
    {
        // arrange.
        var response = SearchByKeywordResponseTestDouble.Create();

        // act.
        SearchResultsViewModel viewModelResults = _serviceModelToViewModelMapper.MapFrom(response);

        // assert.
        for (int i = 0; i < response.EstablishmentResults?.Count; i++)
        {
            Assert.Equal(response.EstablishmentResults.ToList()[i].Urn, viewModelResults.SearchItems![i].Urn);
            Assert.Equal(response.EstablishmentResults.ToList()[i].Name, viewModelResults.SearchItems[i].Name);
            Assert.Equal(response.EstablishmentResults.ToList()[i].Address.Street, viewModelResults.SearchItems[i].Address.Street);
            Assert.Equal(response.EstablishmentResults.ToList()[i].Address.Locality, viewModelResults.SearchItems[i].Address.Locality);
            Assert.Equal(response.EstablishmentResults.ToList()[i].Address.Address3, viewModelResults.SearchItems[i].Address.Address3);
            Assert.Equal(response.EstablishmentResults.ToList()[i].Address.Town, viewModelResults.SearchItems[i].Address.Town);
            Assert.Equal(response.EstablishmentResults.ToList()[i].Address.Postcode, viewModelResults.SearchItems[i].Address.Postcode);
            Assert.Equal(response.EstablishmentResults.ToList()[i].EstablishmentType, viewModelResults.SearchItems[i].EstablishmentType);
            Assert.Equal(response.EstablishmentResults.ToList()[i].PhaseOfEducation, viewModelResults.SearchItems[i].PhaseOfEducation);
            Assert.Equal(response.EstablishmentResults.ToList()[i].EstablishmentStatusName, viewModelResults.SearchItems[i].EstablishmentStatusName);
        }
    }

    [Fact]
    public void Mapper_WithFacetResults_ReturnsFacetsInViewModel()
    {
        // arrange.
        var response = SearchByKeywordResponseTestDouble.Create();

        // act.
        SearchResultsViewModel viewModelResults = _serviceModelToViewModelMapper.MapFrom(response);

        // assert - get rid of this in favourt of thr foreach below
        //for(int i = 0; i < response.EstablishmentFacetResults?.Count; i++) // for each FacetedField (e.g. Phase of education)
        //{
        //    Assert.Equal(response.EstablishmentFacetResults.ToList()[i].Name, viewModelResults.Facets![i].Name); // the name has been mapped correctly

        //    foreach(var expectedFacet in response.EstablishmentFacetResults.ToList()[i].Results) // for each facet (value) within this faceted field (e.g. 'primary')
        //    {
        //        var equivalentFacet = viewModelResults.Facets[i].Values.Where(x => x.Value == expectedFacet.Value).First(); // find the equivalent facet in the mapped response
        //        Assert.NotNull(equivalentFacet);
        //        Assert.Equal(expectedFacet.Count, equivalentFacet.Count);
        //    }
        //}

        // assert
        foreach (var facetedField in response.EstablishmentFacetResults!) // for each FacetedField (e.g. Phase of education)
        {
            var equivalentFacetedField = viewModelResults.Facets!.Where(x => x.Name == facetedField.Name).First();
            Assert.NotNull(equivalentFacetedField); // the name has been mapped correctly

            foreach (var expectedFacet in facetedField.Results) // for each facet (value) within this faceted field (e.g. 'primary')
            {
                var equivalentFacet = equivalentFacetedField.Values.Where(x => x.Value == expectedFacet.Value).First(); // find the equivalent facet in the mapped response
                Assert.NotNull(equivalentFacet);
                Assert.Equal(expectedFacet.Count, equivalentFacet.Count);
            }
        }
    }

    [Fact]
    public void Mapper_NoResultsAndNoFacets_ReturnViewModel_NullEstablishmentResultsAndNullFacets()
    {
        // arrange.
        var establishmentResults = SearchByKeywordResponseTestDouble.CreateWithNoResults();

        // act.
        var viewModelResults = _serviceModelToViewModelMapper.MapFrom(establishmentResults);

        // assert.
        Assert.Null(viewModelResults.SearchItems);
        Assert.Null(viewModelResults.Facets);
    }
}
