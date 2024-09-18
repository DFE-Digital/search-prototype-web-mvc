using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Web.Mappers;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;
using Dfe.Data.SearchPrototype.Web.ViewModels;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.Mappers;

public class SearchByKeywordResponseToViewModelMapperTests
{
    private readonly IMapper<(EstablishmentFacets?, Dictionary<string, List<string>>?), List<Facet>?> _establishmentFacetsToFacetsViewModelMapper
        = new EstablishmentFacetsToFacetsViewModelMapper();

    private readonly IMapper<EstablishmentResults?, List<ViewModels.Establishment>?> _establishmentResultsToEstablishmentsViewModelMapper
        = new EstablishmentResultsToEstablishmentsViewModelMapper();

    [Fact]
    public void Mapper_WithEstablishmentResults_ReturnsEstablishmentResultsInViewModel()
    {
        // arrange.
        var response = SearchByKeywordResponseTestDouble.Create();

        // act.
        ViewModels.SearchResults viewModelResults = new()
        {
            SearchItems =
                _establishmentResultsToEstablishmentsViewModelMapper.MapFrom(response.EstablishmentResults)
        };

        // assert.
        for (int i = 0; i < response.EstablishmentResults!.Establishments?.Count; i++)
        {
            Assert.Equal(response.EstablishmentResults.Establishments.ToList()[i].Urn, viewModelResults.SearchItems![i].Urn);
            Assert.Equal(response.EstablishmentResults.Establishments.ToList()[i].Name, viewModelResults.SearchItems[i].Name);
            Assert.Equal(response.EstablishmentResults.Establishments.ToList()[i].Address.Street, viewModelResults.SearchItems[i].Address.Street);
            Assert.Equal(response.EstablishmentResults.Establishments.ToList()[i].Address.Locality, viewModelResults.SearchItems[i].Address.Locality);
            Assert.Equal(response.EstablishmentResults.Establishments.ToList()[i].Address.Address3, viewModelResults.SearchItems[i].Address.Address3);
            Assert.Equal(response.EstablishmentResults.Establishments.ToList()[i].Address.Town, viewModelResults.SearchItems[i].Address.Town);
            Assert.Equal(response.EstablishmentResults.Establishments.ToList()[i].Address.Postcode, viewModelResults.SearchItems[i].Address.Postcode);
            Assert.Equal(response.EstablishmentResults.Establishments.ToList()[i].EstablishmentType, viewModelResults.SearchItems[i].EstablishmentType);
            Assert.Equal(response.EstablishmentResults.Establishments.ToList()[i].PhaseOfEducation, viewModelResults.SearchItems[i].PhaseOfEducation);
            Assert.Equal(response.EstablishmentResults.Establishments.ToList()[i].EstablishmentStatusName, viewModelResults.SearchItems[i].EstablishmentStatusName);
        }
    }

    //[Fact]
    //public void Mapper_WithFacetResults_ReturnsFacetsInViewModel()
    //{
    //    // arrange.
    //    var response = SearchByKeywordResponseTestDouble.Create();

    //    // act.
    //    ViewModels.SearchResults viewModelResults = new()
    //    {
    //        Facets =
    //            _establishmentFacetsToFacetsViewModelMapper.MapFrom(response.EstablishmentFacetResults, )
    //    };

    //    // assert
    //    foreach (var facetedField in response.EstablishmentFacetResults!.Facets!) // for each FacetedField (e.g. Phase of education)
    //    {
    //        var equivalentFacetedField = viewModelResults.Facets!.Where(x => x.Name == facetedField.Name).First();
    //        Assert.NotNull(equivalentFacetedField); // the name has been mapped correctly

    //        foreach (var expectedFacet in facetedField.Results) // for each facet (value) within this faceted field (e.g. 'primary')
    //        {
    //            var equivalentFacet = equivalentFacetedField.Values.Where(x => x.Value == expectedFacet.Value).First(); // find the equivalent facet in the mapped response
    //            Assert.NotNull(equivalentFacet);
    //            Assert.Equal(expectedFacet.Count, equivalentFacet.Count);
    //        }
    //    }
    //}

    //[Fact]
    //public void Mapper_NoResultsAndNoFacets_ReturnViewModel_NullEstablishmentResultsAndNullFacets()
    //{
    //    // arrange.
    //    var establishmentResults = SearchByKeywordResponseTestDouble.CreateWithNoResults();

    //    // act.
    //    var viewModelResults = _serviceModelToViewModelMapper.MapFrom(establishmentResults);

    //    // assert.
    //    Assert.Null(viewModelResults.SearchItems);
    //    Assert.Null(viewModelResults.Facets);
    //}
}
