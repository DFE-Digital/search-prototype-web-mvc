namespace Dfe.Data.SearchPrototype.Web.Services
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PaginationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="recordCount"></param>
        /// <param name="recordsPerPage"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public int GetTotalNumberOfPages(int totalNumberOfRecords, int recordsPerPage)
        {
            if (totalNumberOfRecords == 0)
                throw new ArgumentException("The record count must be greater than zero.");

            if (recordsPerPage == 0)
                throw new ArgumentException("The page size must be greater than zero.");

            return totalNumberOfRecords / recordsPerPage + (totalNumberOfRecords % recordsPerPage > 0 ? 1 : 0);
        }
    }
}
