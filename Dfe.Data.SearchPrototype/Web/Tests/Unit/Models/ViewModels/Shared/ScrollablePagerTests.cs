using Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared;
using FluentAssertions;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.Models.ViewModels.Shared
{
    public sealed class ScrollablePagerTests
    {
        [Fact]
        public void IsCurrentPageInUpperPagingBoundary_CurrentPageInUpperBoundary_ReturnsTrue()
        {
            // arrange
            ScrollablePager scrollablePager = new();

            // act
            bool result =
                scrollablePager
                    .IsCurrentPageInUpperPagingBoundary(currentPageNumber: 12, totalNumberOfPages: 13);

            // assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsCurrentPageOnUpperPagingThreshold_CurrentPageOnUpperThreshold_ReturnsTrue()
        {
            // arrange
            ScrollablePager scrollablePager = new();

            // act
            bool result =
                scrollablePager
                    .IsCurrentPageInUpperPagingThreshold(currentPageNumber: 10, totalNumberOfPages: 13);

            // assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsCurrentPageInUpperPagingBoundary_TotalPageNumberEqualsPageSequenceWidth_ReturnsTrue()
        {
            // arrange
            ScrollablePager scrollablePager = new();

            // act
            bool result =
                scrollablePager
                    .IsCurrentPageInUpperPagingBoundary(currentPageNumber: 1, totalNumberOfPages: 5);

            // assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsCurrentPageOnUpperPagingThreshold_TotalPageNumberEqualsPageSequenceWidth_ReturnsTrue()
        {
            // arrange
            ScrollablePager scrollablePager = new();

            // act
            bool result =
                scrollablePager
                    .IsCurrentPageInUpperPagingThreshold(currentPageNumber: 1, totalNumberOfPages: 5);

            // assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsCurrentPageInUpperPagingBoundary_CurrentPageNotInUpperBoundary_ReturnsFalse()
        {
            // arrange
            ScrollablePager scrollablePager = new();

            // act
            bool result =
                scrollablePager
                    .IsCurrentPageInUpperPagingBoundary(currentPageNumber: 8, totalNumberOfPages: 13);

            // assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsCurrentPageInUpperPagingThreshold_CurrentPageNotInUpperThreshold_ReturnsFalse()
        {
            // arrange
            ScrollablePager scrollablePager = new();

            // act
            bool result =
                scrollablePager
                    .IsCurrentPageInUpperPagingThreshold(currentPageNumber: 9, totalNumberOfPages: 13);

            // assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsCurrentPageInLowerPagingBoundary_CurrentPageInLowerBoundary_ReturnsTrue()
        {
            // arrange
            ScrollablePager scrollablePager = new();

            // act
            bool result =
                scrollablePager
                    .IsCurrentPageInLowerPagingBoundary(currentPageNumber: 1, totalNumberOfPages: 13);

            // assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsCurrentPageInLowerPagingThreshold_CurrentPageInLowerPagingThreshold_ReturnsTrue()
        {
            // arrange
            ScrollablePager scrollablePager = new();

            // act
            bool result =
                scrollablePager
                    .IsCurrentPageInLowerPagingThreshold(currentPageNumber: 2, totalNumberOfPages: 13);

            // assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsCurrentPageInLowerPagingBoundary_CurrentPageNotInLowerBoundary_ReturnsFalse()
        {
            // arrange
            ScrollablePager scrollablePager = new();

            // act
            bool result =
                scrollablePager
                    .IsCurrentPageInLowerPagingBoundary(currentPageNumber: 4, totalNumberOfPages: 13);

            // assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsCurrentPageInLowerPagingThreshold_CurrentPageNotInLowerPagingThreshold_ReturnsFalse()
        {
            // arrange
            ScrollablePager scrollablePager = new();

            // act
            bool result =
                scrollablePager
                    .IsCurrentPageInLowerPagingThreshold(currentPageNumber: 5, totalNumberOfPages: 13);

            // assert
            result.Should().BeFalse();
        }

        [Fact]
        public void GetPageSequence_CurrentPageWithinUpperBoundary_ReturnsExpectedPageSequence()
        {
            // arrange
            ScrollablePager scrollablePager = new();

            // act
            int[] result = scrollablePager.GetPageSequence(currentPageNumber: 50, totalNumberOfPages: 52);

            // assert
            result.Should().HaveCount(5).And.Equal([48, 49, 50, 51, 52]);
        }

        [Fact]
        public void GetPageSequence_CurrentPageWithinLowerBoundary_ReturnsExpectedPageSequence()
        {
            // arrange
            ScrollablePager scrollablePager = new();

            // act
            int[] result = scrollablePager.GetPageSequence(currentPageNumber: 2, totalNumberOfPages: 52);

            // assert
            result.Should().HaveCount(5).And.Equal([1, 2, 3, 4, 5]);
        }

        [Fact]
        public void GetPageSequence_CurrentPageOutsideOfLowerAndUpperBoundaries_ReturnsExpectedPageSequence()
        {
            // arrange
            ScrollablePager scrollablePager = new();

            // act
            int[] result = scrollablePager.GetPageSequence(currentPageNumber: 22, totalNumberOfPages: 52);

            // assert
            result.Should().HaveCount(5).And.Equal([20, 21, 22, 23, 24]);
        }
    }
}
