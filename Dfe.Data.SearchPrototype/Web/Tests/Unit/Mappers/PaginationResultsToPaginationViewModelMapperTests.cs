using Dfe.Data.SearchPrototype.Infrastructure.Tests.TestDoubles.Shared;
using Dfe.Data.SearchPrototype.Web.Mappers;
using Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared;
using Dfe.Data.SearchPrototype.Web.Options;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.Mappers
{
    public sealed class PaginationResultsToPaginationViewModelMapperTests
    {
        private readonly PaginationResultsToPaginationViewModelMapper _mapper;
        private PaginationOptions _options = new();

        public PaginationResultsToPaginationViewModelMapperTests()
        {
            IOptions<PaginationOptions> paginationOptions =
                IOptionsTestDouble.IOptionsMockFor(_options);

            _mapper = new PaginationResultsToPaginationViewModelMapper(new ScrollablePager(), paginationOptions);
        }

        [Fact]
        public void MapFrom_PopulatesPaginationAsExpected()
        {
            // arrange
            const int currentPage = 1;
            const int totalRecordCount = 113;
            _options.RecordsPerPage = 10;

            // act
            Pagination response = _mapper.MapFrom(input: (currentPage, totalRecordCount));

            // assert
            response.Should().NotBeNull();
            response.CurrentPageNumber.Should().Be(currentPage);
            response.TotalRecordCount.Should().Be(totalRecordCount);
            response.RecordsPerPage.Should().Be(10);
        }
    }
}
