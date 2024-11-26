using Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.Models.ViewModels.Shared
{
    public sealed class PaginationTests
    {
        [Theory]
        [InlineData(2, 1)]
        [InlineData(1, null)]
        public void PreviousPageNumber_ReturnsExpected(int currentPageNumber, int? expected)
        {
            // arrange
            Pagination pagination = new(currentPageNumber, totalRecordCount : 30, recordsPerPage:10);

            // act
            int? result = pagination.PreviousPageNumber;

            // assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(120, 10, 11, 12)]
        [InlineData(120, 10, 12, null)]
        public void NextPageNumber_ReturnsExpected(
            int totalRecordCount,
            int recordsPerPage,
            int currentPageNumber,
            int? expected)
        {
            // arrange
            Pagination pagination = new(currentPageNumber, totalRecordCount, recordsPerPage);

            // act
            int? result = pagination.NextPageNumber;

            // assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(123, 10, 13)]
        [InlineData(9, 10, 1)]
        [InlineData(0, 10, 0)]
        [InlineData(10, 0, 0)]
        public void TotalNumberOfPages_ReturnsExpected(int totalRecordCount,
            int recordsPerPage,
            int? expected)
        {
            // arrange
            Pagination pagination = new(currentPageNumber : 1, totalRecordCount, recordsPerPage);

            // act
            int result = pagination.TotalNumberOfPages;

            // assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(20, 10, true)]
        [InlineData(10, 10, false)]
        [InlineData(9, 10, false)]
        public void IsPageable_ReturnsExpected(int totalRecordCount,
            int recordsPerPage,
            bool expected)
        {
            // arrange
            Pagination pagination = new(currentPageNumber: 1, totalRecordCount, recordsPerPage);

            // act
            bool result = pagination.IsPageable;

            // assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(20, 10, 2, true)]
        [InlineData(20, 10, 1, false)]
        public void IsLastPage_ReturnsExpected(
            int totalRecordCount,
            int recordsPerPage,
            int currentPageNumber,
            bool expected)
        {
            // arrange
            Pagination pagination = new(currentPageNumber, totalRecordCount, recordsPerPage);

            // act
            bool result = pagination.IsLastPage;

            // assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(20, 10, 1, true)]
        [InlineData(20, 10, 2, false)]
        public void IsFirstPage_ReturnsExpected(int totalRecordCount,
            int recordsPerPage,
            int currentPageNumber,
            bool expected)
        {
            // arrange
            Pagination pagination = new(currentPageNumber, totalRecordCount, recordsPerPage);

            // act
            bool result = pagination.IsFirstPage;

            // assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(9, 130, true)]
        [InlineData(10, 130, false)]
        [InlineData(1, 50, false)]
        [InlineData(2, 30, false)]
        public void HasMoreUpperPagesAvailable_ReturnsExpected(int currentPageNumber, int totalRecordCount, bool expected)
        {
            // arrange
            Pagination pagination = new(currentPageNumber, totalRecordCount, recordsPerPage:10);

            // act
            bool result =
                pagination
                    .HasMoreUpperPagesAvailable;

            // assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(8, 130, false)]
        [InlineData(10, 130, false)]
        [InlineData(12, 130, true)]
        [InlineData(1, 50, true)]
        public void PageSequenceIncludesLastPage_ReturnsExpected(int currentPageNumber, int totalRecordCount, bool expected)
        {
            // arrange
            Pagination pagination = new(currentPageNumber, totalRecordCount, recordsPerPage: 10);

            // act
            bool result =
                pagination
                    .PageSequenceIncludesLastPage;

            // assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(1, 130, true)]
        [InlineData(4, 130, false)]
        public void PageSequenceIncludesFirstPage_ReturnsExpected(int currentPageNumber, int totalRecordCount, bool expected)
        {
            // arrange
            Pagination pagination = new(currentPageNumber, totalRecordCount, recordsPerPage: 10);

            // act
            bool result =
                pagination
                    .PageSequenceIncludesFirstPage;

            // assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(2, 130, false)]
        [InlineData(3, 130, false)]
        [InlineData(4, 130, false)]
        [InlineData(5, 130, true)]
        public void HasMoreLowerPagesAvailable_ReturnsExpected(int currentPageNumber, int totalRecordCount, bool expected)
        {
            // arrange
            Pagination pagination = new(currentPageNumber, totalRecordCount, recordsPerPage: 10);

            // act
            bool result =
                pagination
                    .HasMoreLowerPagesAvailable;

            // assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(50, 520, new int[] { 48, 49, 50, 51, 52 })]
        [InlineData(51, 520, new int[] { 48, 49, 50, 51, 52 })]
        [InlineData(2, 520, new int[] { 1, 2, 3, 4, 5 })]
        [InlineData(3, 520, new int[] { 1, 2, 3, 4, 5 })]
        [InlineData(22, 520, new int[] { 20, 21, 22, 23, 24 })]
        public void GetPageSequence_ReturnsExpected(
            int currentPageNumber, int totalRecordCount, int[] expected)
        {
            // arrange
            Pagination pagination = new(currentPageNumber, totalRecordCount, recordsPerPage: 10);

            // act
            int[] result = pagination.CurrentPageSequence;

            // assert
            result.Should().Equal(expected);
        }
    }
}
