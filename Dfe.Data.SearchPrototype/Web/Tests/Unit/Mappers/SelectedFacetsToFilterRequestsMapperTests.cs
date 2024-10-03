using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.Web.Mappers;
using FluentAssertions;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.Mappers;

public class SelectedFacetsToFilterRequestsMapperTests
{
    private readonly SelectedFacetsToFilterRequestsMapper _mapper = new SelectedFacetsToFilterRequestsMapper();

    [Fact]
    public void Mapper_WithFacetsInput_ReturnsFilterRequests()
    {
        // arrange
        Dictionary<string, List<string>>? facetSelections = new Dictionary<string, List<string>>
        {
            { "FacetName1", new List<string> {"FacetValue1a", "FacetValue1b" } },
            { "FacetName2", new List<string> {"FacetValue2a", "FacetValue2b" } }
        };

        // act
        IList<FilterRequest>? response = _mapper.MapFrom(input: facetSelections);

        // assert
        response!.Single(filterRequest => filterRequest.FilterName == "FacetName1")
            .FilterValues
            .Should().BeEquivalentTo(facetSelections["FacetName1"]);
        response!.Single(filterRequest => filterRequest.FilterName == "FacetName2")
            .FilterValues
            .Should().BeEquivalentTo(facetSelections["FacetName2"]);
    }

    [Fact]
    public void Mapper_NoInput_ReturnsEmptyList()
    {
        // act
        IList<FilterRequest> response = _mapper.MapFrom(null);

        // assert
        response.Should().BeEmpty();
    }
}
