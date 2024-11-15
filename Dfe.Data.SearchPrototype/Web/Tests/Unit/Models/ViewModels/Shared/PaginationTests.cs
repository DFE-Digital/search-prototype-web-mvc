using Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.Models.ViewModels.Shared
{
    public sealed class PaginationTests
    {
        [Fact]
        public void PreviousPageNumber_WithCurrentPageGreaterThanOne_ReturnsPrevious()
        {
            // arrange
            Pagination pagination = new(new Mock<IPager>().Object)
            {
                CurrentPageNumber = 2
            };

            // act
            int? result = pagination.PreviousPageNumber;

            // assert
            result.Should().Be(1);
        }

        [Fact]
        public void PreviousPageNumber_WithCurrentPageEqualToOne_ReturnsOne()
        {
            // arrange
            Pagination pagination = new(new Mock<IPager>().Object)
            {
                CurrentPageNumber = 1
            };

            // act
            int? result = pagination.PreviousPageNumber;
            bool isFirstPage = pagination.IsFirstPage;
            // assert
            result.Should().Be(null);
            isFirstPage.Should().BeTrue();
        }

        [Fact]
        public void NextPageNumber_WithCurrentPageNotLast_ReturnsNext()
        {
            // arrange
            Pagination pagination = new(new Mock<IPager>().Object)
            {
                TotalRecordCount = 120,
                RecordsPerPage = 10,
                CurrentPageNumber = 11
            };

            // act
            int? result = pagination.NextPageNumber;

            // assert
            result.Should().Be(12);
        }

        [Fact]
        public void NextPageNumber_WithCurrentPageEqualToLast_ReturnsLastPage()
        {
            // arrange
            Pagination pagination = new(new Mock<IPager>().Object)
            {
                TotalRecordCount = 120,
                RecordsPerPage = 10,
                CurrentPageNumber = 12
            };

            // act
            int? result = pagination.NextPageNumber;
            bool isLastPage = pagination.IsLastPage; 
            // assert
            result.Should().Be(null);
            isLastPage.Should().BeTrue();
        }

        [Fact]
        public void TotalNumberOfPages_WithTotalRecordsGreaterThanRecordsPerPage_ReturnsMultiplePages()
        {
            // arrange
            Pagination pagination = new(new Mock<IPager>().Object)
            {
                TotalRecordCount = 123,
                RecordsPerPage = 10
            };

            // act
            int result = pagination.TotalNumberOfPages;

            // assert
            result.Should().Be(13);
        }

        [Fact]
        public void TotalNumberOfPages_WithTotalRecordsLessThanRecordsPerPage_ReturnsOnePage()
        {
            // arrange
            Pagination pagination = new(new Mock<IPager>().Object)
            {
                TotalRecordCount = 9,
                RecordsPerPage = 10
            };

            // act
            int result = pagination.TotalNumberOfPages;

            // assert
            result.Should().Be(1);
        }

        [Fact]
        public void TotalNumberOfPages_WithTotalRecordCountZero_ThrowsArgumentException()
        {
            // arrange
            Pagination pagination = new(new Mock<IPager>().Object)
            {
                TotalRecordCount = 0,
                RecordsPerPage = 10
            };

            // act, assert
            Action failedAction =
                () => _ = pagination.TotalNumberOfPages;

            ArgumentException exception = Assert.Throws<ArgumentException>(failedAction);

            exception.Message.Should().Be("The record count must be greater than zero.");
        }

        [Fact]
        public void TotalNumberOfPages_WithRecordsPerPageZero_ThrowsArgumentException()
        {
            // arrange
            Pagination pagination = new(new Mock<IPager>().Object)
            {
                TotalRecordCount = 10,
                RecordsPerPage = 0
            };

            // act, assert
            Action failedAction =
                () => _ = pagination.TotalNumberOfPages;

            ArgumentException exception = Assert.Throws<ArgumentException>(failedAction);

            exception.Message.Should().Be("The page size must be greater than zero.");
        }

        [Fact]
        public void IsPageable_WithMoreThanOnePage_ReturnsTrue()
        {
            // arrange
            Mock<IPager> mockPager = new();
            mockPager.Setup(pager => pager.GetPageSequence(It.IsAny<int>(), It.IsAny<int>())).Returns([1, 2, 3, 4, 5]);

            Pagination pagination = new(mockPager.Object)
            {
                TotalRecordCount = 20,
                RecordsPerPage = 10
            };

            // act
            bool result = pagination.IsPageable;

            // assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsPageable_WithExactlyOnePage_ReturnsFalse()
        {
            // arrange
            Mock<IPager> mockPager = new();
            mockPager.Setup(pager => pager.GetPageSequence(It.IsAny<int>(), It.IsAny<int>())).Returns([1]);

            Pagination pagination = new(mockPager.Object)
            {
                TotalRecordCount = 10,
                RecordsPerPage = 10
            };

            // act
            bool result = pagination.IsPageable;

            // assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsLastPage_WithCurrentPageSetToLast_ReturnsTrue()
        {
            // arrange

            // arrange
            Pagination pagination = new(new Mock<IPager>().Object)
            {
                TotalRecordCount = 20,
                RecordsPerPage = 10,
                CurrentPageNumber = 2
            };

            // act
            bool result = pagination.IsLastPage;

            // assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsLastPage_WithCurrentPageNotSetToLast_ReturnsFalse()
        {
            // arrange

            // arrange
            Pagination pagination = new(new Mock<IPager>().Object)
            {
                TotalRecordCount = 20,
                RecordsPerPage = 10,
                CurrentPageNumber = 1
            };

            // act
            bool result = pagination.IsLastPage;

            // assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsFirstPage_WithCurrentPageSetToFirst_ReturnsTrue()
        {
            // arrange

            // arrange
            Pagination pagination = new(new Mock<IPager>().Object)
            {
                TotalRecordCount = 20,
                RecordsPerPage = 10,
                CurrentPageNumber = 1
            };

            // act
            bool result = pagination.IsFirstPage;

            // assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsFirstPage_WithCurrentPageNotSetToFirst_ReturnsFalse()
        {
            // arrange

            // arrange
            Pagination pagination = new(new Mock<IPager>().Object)
            {
                TotalRecordCount = 20,
                RecordsPerPage = 10,
                CurrentPageNumber = 2
            };

            // act
            bool result = pagination.IsFirstPage;

            // assert
            result.Should().BeFalse();
        }
    }
}
