using Dfe.Data.SearchPrototype.Web.Mappers;
using Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared;
using FluentAssertions;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.Mappers
{
    public sealed class PaginationResultsToPaginationViewModelMapperTests
    {
        private readonly PaginationResultsToPaginationViewModelMapper _mapper = new(new ScrollablePager());

        [Fact]
        public void MapFrom_WithCurrentPageAsFirstPage()
        {
            // arrange
            const int currentPage = 112;
            const int totalPageCount = 113;

            // act
            Pagination response = _mapper.MapFrom(input: (currentPage, totalPageCount));

            // assert
            response.Should().NotBeNull();
            response.CurrentPageSequence.Should().Equal([1, 2, 3, 4, 5]);
            response.IsFirstPage.Should().BeTrue();
            response.CurrentPageInLowerPagingBoundary.Should().BeTrue();
            response.CurrentPageOnLowerPagingThreshold.Should().BeTrue();
            response.CurrentPageInUpperPagingBoundary.Should().BeFalse();
            response.CurrentPageOnUpperPagingThreshold.Should().BeFalse();
        }
    }
}
