using Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared;
using FluentAssertions;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.Models.ViewModels.Shared
{
    public sealed class PaginationTests
    {
        //[Fact]
        //public void GetTotalNumberOfPages()
        //{
        //    Pagination pagination = new();
        //    pagination.TotalRecordCount = 92;
        //    pagination.RecordsPerPage = 20;
        //    int totalNumberOfPages = pagination.TotalNumberOfPages;
        //    totalNumberOfPages.Should().Be(5);
        //}

        //[Fact]
        //public void GetPageSequence_WithFullLeftPadding()
        //{
        //    Pagination pagination = new();
        //    pagination.TotalRecordCount = 1023;
        //    pagination.RecordsPerPage = 20;
        //    pagination.CurrentPageNumber = 21;
        //    List<int> pageSequence = pagination.CurrentPageSequence;
        //    pageSequence.Should().Equal([19, 20, 21, 22, 23]);
        //}

        //[Fact]
        //public void GetPageSequence_WithFullRightPadding()
        //{
        //    Pagination pagination = new();
        //    pagination.TotalRecordCount = 1023;
        //    pagination.RecordsPerPage = 20;
        //    pagination.CurrentPageNumber = 50;
        //    List<int> pageSequence = pagination.CurrentPageSequence;
        //    pageSequence.Should().Equal([48, 49, 50, 51, 52]);
        //}

        //[Fact]
        //public void GetPageSequence_WithNoLeftPadding()
        //{
        //    Pagination pagination = new();
        //    pagination.TotalRecordCount = 1023;
        //    pagination.RecordsPerPage = 20;
        //    pagination.CurrentPageNumber = 1;
        //    List<int> pageSequence = pagination.CurrentPageSequence;
        //    pageSequence.Should().Equal([1, 2, 3, 4, 5]);
        //}
        //[Fact]
        //public void GetPageSequence_With1LeftPadding()
        //{
        //    Pagination pagination = new();
        //    pagination.TotalRecordCount = 1023;
        //    pagination.RecordsPerPage = 20;
        //    pagination.CurrentPageNumber = 2;
        //    List<int> pageSequence = pagination.CurrentPageSequence;
        //    pageSequence.Should().Equal([1, 2, 3, 4, 5]);
        //}

        //[Fact]
        //public void GetPageSequence_WithNoRightPadding()
        //{
        //    Pagination pagination = new();
        //    pagination.TotalRecordCount = 1023;
        //    pagination.RecordsPerPage = 20;
        //    pagination.CurrentPageNumber = 52;
        //    List<int> pageSequence = pagination.CurrentPageSequence;
        //    pageSequence.Should().Equal([48, 49, 50, 51, 52]);
        //}

        //[Fact]
        //public void GetPageSequence_With1RightPadding()
        //{
        //    Pagination pagination = new();
        //    pagination.TotalRecordCount = 1023;
        //    pagination.RecordsPerPage = 20;
        //    pagination.CurrentPageNumber = 51;
        //    List<int> pageSequence = pagination.CurrentPageSequence;
        //    pageSequence.Should().Equal([48, 49, 50, 51, 52]);
        //}
    }
}
