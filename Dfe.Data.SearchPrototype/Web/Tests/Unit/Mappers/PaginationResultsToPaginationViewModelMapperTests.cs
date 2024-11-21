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

        public PaginationResultsToPaginationViewModelMapperTests()
        {
            PaginationOptions options = new(){
                RecordsPerPage = 10,
            };

            IOptions<PaginationOptions> paginationOptions =
                IOptionsTestDouble.IOptionsMockFor(options);

            _mapper = new PaginationResultsToPaginationViewModelMapper(new ScrollablePager(), paginationOptions);
        }

        [Fact]
        public void MapFrom_WithCurrentPageAsFirstPage()
        {
            // arrange
            const int currentPage = 1;
            const int totalRecordCount = 113;

            // act
            Pagination response = _mapper.MapFrom(input: (currentPage, totalRecordCount));

            // assert
            response.Should().NotBeNull();
            response.CurrentPageSequence.Should().Equal([1, 2, 3, 4, 5]);
            response.IsFirstPage.Should().BeTrue();
            response.PageSequenceIncludesFirstPage.Should().BeTrue();
            response.HasMoreLowerPagesAvailable.Should().BeFalse();
            response.PageSequenceIncludesLastPage.Should().BeFalse();
            response.HasMoreUpperPagesAvailable.Should().BeTrue();
        }

        [Theory]
        [InlineData(3, 113, new int[] { 1, 2, 3, 4, 5 })]
        [InlineData(2, 113, new int[] { 1, 2, 3, 4, 5 })]
        public void MapFrom_WithCurrentPageInLowerPageBoundary(
            int currentPage, int totalRecordCount, int[] expectedPageSequence)
        {
            // act
            Pagination response = _mapper.MapFrom(input: (currentPage, totalRecordCount));

            // assert
            response.Should().NotBeNull();
            response.CurrentPageSequence.Should().Equal(expectedPageSequence);
            response.IsFirstPage.Should().BeFalse();
            response.PageSequenceIncludesFirstPage.Should().BeTrue();
            response.HasMoreLowerPagesAvailable.Should().BeFalse();
            response.PageSequenceIncludesLastPage.Should().BeFalse();
            response.HasMoreUpperPagesAvailable.Should().BeTrue();
        }

        [Theory]
        [InlineData(12, 113, new int[] { 8, 9, 10, 11, 12 })]
        [InlineData(11, 113, new int[] { 8, 9, 10, 11, 12 })]
        public void MapFrom_WithCurrentPageInUpperPageBoundary(
            int currentPage, int totalRecordCount, int[] expectedPageSequence)
        {
            // act
            Pagination response = _mapper.MapFrom(input: (currentPage, totalRecordCount));
            // assert
            response.Should().NotBeNull();
            response.CurrentPageSequence.Should().Equal(expectedPageSequence);
            response.IsFirstPage.Should().BeFalse();
            response.PageSequenceIncludesFirstPage.Should().BeFalse();
            response.HasMoreLowerPagesAvailable.Should().BeTrue();
            response.PageSequenceIncludesLastPage.Should().BeTrue();
            response.HasMoreUpperPagesAvailable.Should().BeFalse();
        }

        [Theory]
        [InlineData(8, 113, new int[] { 6, 7, 8, 9, 10 })]
        [InlineData(7, 113, new int[] { 5, 6, 7, 8, 9 })]
        public void MapFrom_WithCurrentPageBetweenFirstAndLast2Pages(
            int currentPage, int totalRecordCount, int[] expectedPageSequence)
        {
            // act
            Pagination response = _mapper.MapFrom(input: (currentPage, totalRecordCount));

            // assert
            response.Should().NotBeNull();
            response.CurrentPageSequence.Should().Equal(expectedPageSequence);
            response.IsFirstPage.Should().BeFalse();
            response.PageSequenceIncludesFirstPage.Should().BeFalse();
            response.HasMoreLowerPagesAvailable.Should().BeTrue();
            response.PageSequenceIncludesLastPage.Should().BeFalse();
            response.HasMoreUpperPagesAvailable.Should().BeTrue();
        }
    }
}
