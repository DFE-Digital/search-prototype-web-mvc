namespace Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared
{
    /// <summary>
    /// Provides functionality to allow paging to work in the manner defined
    /// by the underlying implementation following the contract described.
    /// </summary>
    public interface IPager
    {
        /// <summary>
        /// Determines whether the current page falls within the lower paging boundary.
        /// </summary>
        /// <param name="currentPageNumber">
        /// The current page selected through the pagination provisioned.
        /// </param>
        /// <param name="totalNumberOfPages">
        /// The total number of pages available based on the total number of records
        /// and the page sequence width.
        /// </param>
        /// <returns>
        /// True or false based on whether the condition is met (i.e. the
        /// current page falls within the lower paging bounds).
        /// </returns>
        public bool IsCurrentPageInLowerPagingBoundary(int currentPageNumber, int totalNumberOfPages);

        /// <summary>
        /// Determines whether the current page falls on the lower paging threshold.
        /// </summary>
        /// <param name="currentPageNumber">
        /// The current page selected through the pagination provisioned.
        /// </param>
        /// <param name="totalNumberOfPages">
        /// The total number of pages available based on the total number of records
        /// and the page sequence width.
        /// </param>
        /// <returns>
        /// True or false based on whether the condition is met (i.e. the
        /// current page falls in the lower paging threshold).
        /// </returns>
        public bool IsCurrentPageInLowerPagingThreshold(int currentPageNumber, int totalNumberOfPages);

        /// <summary>
        /// Determines whether the current page falls within the upper paging boundary.
        /// </summary>
        /// <param name="currentPageNumber">
        /// The current page selected through the pagination provisioned.
        /// </param>
        /// <param name="totalNumberOfPages">
        /// The total number of pages available based on the total number of records
        /// and the page sequence width.
        /// </param>
        /// <returns>
        /// True or false based on whether the condition is met (i.e. the
        /// current page falls within the upper paging bounds).
        /// </returns>
        public bool IsCurrentPageInUpperPagingBoundary(int currentPageNumber, int totalNumberOfPages);

        /// <summary>
        /// Determines whether the current page falls within the upper paging threshold.
        /// </summary>
        /// <param name="currentPageNumber">
        /// The current page selected through the pagination provisioned.
        /// </param>
        /// <param name="totalNumberOfPages">
        /// The total number of pages available based on the total number of records
        /// and the page sequence width.
        /// </param>
        /// <returns>
        /// True or false based on whether the condition is met (i.e. the
        /// current page falls on the upper paging threshold).
        /// </returns>
        public bool IsCurrentPageInUpperPagingThreshold(int currentPageNumber, int totalNumberOfPages);

        /// <summary>
        /// Determines the current page sequence to return given the current page number and total number of pages.
        /// </summary>
        /// <param name="currentPageNumber">
        /// The current page selected through the pagination provisioned.
        /// </param>
        /// <param name="totalNumberOfPages">
        /// The total number of pages available based on the total number of records
        /// and the page sequence width.
        /// </param>
        /// <returns>
        /// An integer array which represents the pages to be contained within a given pagination sequence.
        /// </returns>
        int[] GetPageSequence(int currentPageNumber, int totalNumberOfPages);
    }
}
