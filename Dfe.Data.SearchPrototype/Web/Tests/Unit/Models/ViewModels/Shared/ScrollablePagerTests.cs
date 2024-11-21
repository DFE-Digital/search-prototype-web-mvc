using Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.Models.ViewModels.Shared
{
    public sealed class ScrollablePagerTests
    {
        [Theory]
        [InlineData(9, 13, true)]
        [InlineData(10, 13, false)]
        [InlineData(1, 5, false)]
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
        [InlineData(10, 13, false)]
        [InlineData(12, 13, true)]
        [InlineData(1, 5, true)]
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
                    .PageSequenceIncludesFirstPage(currentPageNumber, totalNumberOfPages); // TODO CML this shouldn't need to know how many pages there are.

            // assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(2, 13, false)]
        [InlineData(3, 13, false)]
        [InlineData(4, 13, false)]
        [InlineData(5, 13, true)]
        public void HasMoreLowerPagesAvailable_ReturnsExpected(int currentPageNumber, int totalNumberOfPages, bool expected)
        {
            // arrange
            ScrollablePager scrollablePager = new();

            // act
            bool result =
                scrollablePager
                    .HasMoreLowerPagesAvailable(currentPageNumber, totalNumberOfPages); // TODO CML this shouldn't need to know how many pages there are.

            // assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(50, 52, new int[] { 48, 49, 50, 51, 52 })]
        [InlineData(51, 52, new int[] { 48, 49, 50, 51, 52 })]
        [InlineData(2, 52, new int[] { 1, 2, 3, 4, 5 })]
        [InlineData(3, 52, new int[] { 1, 2, 3, 4, 5 })]
        [InlineData(22, 52, new int[] { 20, 21, 22, 23, 24 })]
        public void GetPageSequence_ReturnsExpected(
            int currentPageNumber, int totalNumberOfPages, int[] expected)
        {
            // arrange
            ScrollablePager scrollablePager = new();

            // act
            int[] result = scrollablePager.GetPageSequence(currentPageNumber, totalNumberOfPages);

            // assert
            result.Should().Equal(expected);
        }
    }
}
