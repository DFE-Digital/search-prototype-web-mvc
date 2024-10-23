using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.WebApi.Controllers;
using Dfe.Data.SearchPrototype.WebApi.Mappers;
using FluentAssertions;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.APITests.Unit;

public class SearchRequestToFilterRequestsMapperTests
{
    private IMapper<SearchRequest, IList<FilterRequest>> _mapper = new SearchRequestToFilterRequestsMapper();

    [Fact]
    public void MapFrom_MapsFilters()
    {
        // arrange
        var searchrequest = new SearchRequest()
        {
            EstablishmentStatus = new List<string> { "status1", "status2" },
            PhaseOfEducation = new List<string> { "Phase 1", "Phase 2" }
        };

        // act
        var response = _mapper.MapFrom(searchrequest);

        // assert
        response!.Single(filterRequest => filterRequest.FilterName == "PHASEOFEDUCATION")
            .FilterValues
            .Should().BeEquivalentTo(searchrequest.PhaseOfEducation);
        response!.Single(filterRequest => filterRequest.FilterName == "ESTABLISHMENTSTATUSNAME")
            .FilterValues
            .Should().BeEquivalentTo(searchrequest.EstablishmentStatus);
    }

    [Fact]
    public void MapFrom_NoFilters_NullResponse()
    {
        // arrange
        var searchrequest = new SearchRequest();

        // act
        var response = _mapper.MapFrom(searchrequest);

        // assert
        response.Should().BeNull();
    }

}
