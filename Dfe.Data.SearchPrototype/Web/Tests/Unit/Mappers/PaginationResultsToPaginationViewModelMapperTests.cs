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
            const int currentPage = 1;
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

        [Theory]
        [InlineData(3, 113, new int[] { 1, 2, 3, 4, 5 })]
        [InlineData(2, 113, new int[] { 1, 2, 3, 4, 5 })]
        public void MapFrom_WithCurrentPageAsSecondPage(
            int currentPage, int totalPageCount, int[] expectedPageSequence)
        {
            // act
            Pagination response = _mapper.MapFrom(input: (currentPage, totalPageCount));

            // assert
            response.Should().NotBeNull();
            response.CurrentPageSequence.Should().Equal(expectedPageSequence);
            response.IsFirstPage.Should().BeFalse();
            response.CurrentPageInLowerPagingBoundary.Should().BeTrue();
            response.CurrentPageOnLowerPagingThreshold.Should().BeTrue();
            response.CurrentPageInUpperPagingBoundary.Should().BeFalse();
            response.CurrentPageOnUpperPagingThreshold.Should().BeFalse();
        }

        [Theory]
        [InlineData(111, 113, new int[] { 109, 110, 111, 112, 113 })]
        [InlineData(110, 113, new int[] { 108, 109, 110, 111, 112 })]
        public void MapFrom_WithCurrent(
            int currentPage, int totalPageCount, int[] expectedPageSequence)
        {
            // act
            Pagination response = _mapper.MapFrom(input: (currentPage, totalPageCount));
            // assert
            response.Should().NotBeNull();
            response.CurrentPageSequence.Should().Equal(expectedPageSequence);
            response.IsFirstPage.Should().BeFalse();
            response.CurrentPageInLowerPagingBoundary.Should().BeFalse();
            response.CurrentPageOnLowerPagingThreshold.Should().BeFalse();
            response.CurrentPageInUpperPagingBoundary.Should().BeTrue();
            response.CurrentPageOnUpperPagingThreshold.Should().BeTrue();
        }

        [Theory]
        [InlineData(113, 113, new int[] {109 ,110 ,111 , 112, 113})]
        [InlineData(112, 113, new int[] {109, 110, 111, 112, 113})]
        public void MapFrom_With(
            int currentPage, int totalPageCount, int[] expectedPageSequence)
        {
            // act
            Pagination response = _mapper.MapFrom(input: (currentPage, totalPageCount));

            // assert
            response.Should().NotBeNull();
            response.CurrentPageSequence.Should().Equal(expectedPageSequence);
            response.IsFirstPage.Should().BeFalse();
            response.CurrentPageInLowerPagingBoundary.Should().BeFalse();
            response.CurrentPageOnLowerPagingThreshold.Should().BeFalse();
            response.CurrentPageInUpperPagingBoundary.Should().BeTrue();
            response.CurrentPageOnUpperPagingThreshold.Should().BeTrue();
        }

        [Theory]
        [InlineData(4, 113, new int[] { 2, 3, 4, 5, 6 })]
        [InlineData(5, 113, new int[] { 3, 4, 5, 6, 7 })]
        public void MapFrom_WithCurrentPageBetweenFirstAndLast2(
            int currentPage, int totalPageCount, int[] expectedPageSequence)
        {
            // act
            Pagination response = _mapper.MapFrom(input: (currentPage, totalPageCount));

            // assert
            response.Should().NotBeNull();
            response.CurrentPageSequence.Should().Equal(expectedPageSequence);
            response.IsFirstPage.Should().BeFalse();
            response.CurrentPageInLowerPagingBoundary.Should().BeFalse();
            response.CurrentPageOnLowerPagingThreshold.Should().BeTrue();
            response.CurrentPageInUpperPagingBoundary.Should().BeFalse();
            response.CurrentPageOnUpperPagingThreshold.Should().BeFalse();
        }
    }
}
