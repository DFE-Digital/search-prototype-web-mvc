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
            Pagination pagination = new(new Mock<IPager>().Object)
            {
                CurrentPageNumber = currentPageNumber
            };

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
            Pagination pagination = new(new Mock<IPager>().Object)
            {
                TotalRecordCount = totalRecordCount,
                RecordsPerPage = recordsPerPage,
                CurrentPageNumber = currentPageNumber
            };

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
            Pagination pagination = new(new Mock<IPager>().Object)
            {
                TotalRecordCount = totalRecordCount,
                RecordsPerPage = recordsPerPage
            };

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
            Mock<IPager> mockPager = new();
            mockPager.Setup(pager => pager.GetPageSequence(It.IsAny<int>(), It.IsAny<int>())).Returns([1, 2, 3, 4, 5]);

            Pagination pagination = new(mockPager.Object)
            {
                TotalRecordCount = totalRecordCount,
                RecordsPerPage = recordsPerPage
            };

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
            Pagination pagination = new(new Mock<IPager>().Object)
            {
                TotalRecordCount = totalRecordCount,
                RecordsPerPage = recordsPerPage,
                CurrentPageNumber = currentPageNumber
            };

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
            Pagination pagination = new(new Mock<IPager>().Object)
            {
                TotalRecordCount = totalRecordCount,
                RecordsPerPage = recordsPerPage,
                CurrentPageNumber = currentPageNumber
            };

            // act
            bool result = pagination.IsFirstPage;

            // assert
            result.Should().Be(expected);
        }
    }
}
