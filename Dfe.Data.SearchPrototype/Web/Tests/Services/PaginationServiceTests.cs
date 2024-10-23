using Dfe.Data.SearchPrototype.Web.Services;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.Services
{
    public sealed class PaginationServiceTests
    {
        [Fact]
        public void xxx()
        {
            int totalNumberOfPages = new PaginationService().GetTotalNumberOfPages(totalNumberOfRecords: 92, recordsPerPage: 20);
        }
    }
}
