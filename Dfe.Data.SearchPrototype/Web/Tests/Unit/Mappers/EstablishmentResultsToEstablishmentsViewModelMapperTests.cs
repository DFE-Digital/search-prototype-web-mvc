using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Web.Mappers;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.Mappers;

public class EstablishmentResultsToEstablishmentsViewModelMapperTests
{
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

    [Fact]
    public void Mapper_NoEstablishmentsResults_ReturnNullViewModel_NullEstablishments()
    {
        // arrange.
        var response = SearchByKeywordResponseTestDouble.CreateWithNoResults();

        // act.
        var viewModelResults = new ViewModels.SearchResults()
        {
            SearchItems =
                _establishmentResultsToEstablishmentsViewModelMapper.MapFrom(response.EstablishmentResults)
        };

        // assert.
        Assert.Null(viewModelResults.SearchItems);
    }
}
