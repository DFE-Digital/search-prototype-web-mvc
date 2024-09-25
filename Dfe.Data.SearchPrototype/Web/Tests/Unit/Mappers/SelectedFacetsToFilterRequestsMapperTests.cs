using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.Web.Mappers;
using Dfe.Data.SearchPrototype.Web.Tests.Unit.TestDoubles;
using Xunit;
using FluentAssertions;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.Mappers
{
    public class SelectedFacetsToFilterRequestsMapperTests
    {
        private readonly IMapper<Dictionary<string, List<string>>, IList<FilterRequest>> _mapper;

        public SelectedFacetsToFilterRequestsMapperTests() => _mapper = new SelectedFacetsToFilterRequestsMapper();

        [Fact]
        public void Mapper_WithFacetedResultsViewModel_ReturnsFilterRequest()
        {
            // arrange.
            Dictionary<string, List<string>> viewModel = FacetsSelectedModelTestDouble.Create();

            // act.
            IList<FilterRequest> response = _mapper.MapFrom(input: viewModel);

            // assert.
            response.Should().NotBeEmpty().And.AllBeOfType<FilterRequest>().And.HaveCountGreaterThan(0);
        }
    }
}
