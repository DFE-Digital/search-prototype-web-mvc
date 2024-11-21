using Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared;
using FluentAssertions;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.Models.ViewModels.Shared
{
    public sealed class ScrollablePagerTests
    {
        [Theory]
        [InlineData(9,13,true)]
        [InlineData(1, 5, false)]
        [InlineData(10,13,false)]
        public void HasMoreUpperPagesAvailable_ReturnsExpected(int currentPageNumber, int totalNumberOfPages, bool expected)
        {
            // arrange
            ScrollablePager scrollablePager = new();

            // act
            bool result =
                scrollablePager
                    .HasMoreUpperPagesAvailable(currentPageNumber, totalNumberOfPages);

            // assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(8,13,false)]
        [InlineData(1, 5, true)]
        [InlineData(12,13, true)]
        public void PageSequenceIncludesLastPage_ReturnsExpected(int currentPageNumber, int totalNumberOfPages, bool expected)
        {
            // arrange
            ScrollablePager scrollablePager = new();

            // act
            bool result =
                scrollablePager
                    .PageSequenceIncludesLastPage(currentPageNumber, totalNumberOfPages);

            // assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(1,13,true)]
        [InlineData(4,13,false)]
        public void PageSequenceIncludesFirstPage_ReturnsExpected(int currentPageNumber, int totalNumberOfPages, bool expected )
        {
            // arrange
            ScrollablePager scrollablePager = new();

            // act
            bool result =
                scrollablePager
                    .PageSequenceIncludesFirstPage(currentPageNumber, totalNumberOfPages);

            // assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(2,13,false)]
        [InlineData(5,13,true)]
        public void HasMoreLowerPagesAvailable_ReturnsExpected(int currentPageNumber, int totalNumberOfPages, bool expected)
        {
            // arrange
            ScrollablePager scrollablePager = new();

            // act
            bool result =
                scrollablePager
                    .HasMoreLowerPagesAvailable(currentPageNumber, totalNumberOfPages);

            // assert
            result.Should().Be(expected);
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
