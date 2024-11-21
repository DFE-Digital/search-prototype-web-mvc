namespace Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared
{
    /// <summary>
    /// View model to describe and assign pagination related presentation concerns.
    /// </summary>
    public sealed class Pagination(IPager _pager)
    {
        /// <summary>
        /// Establishes the current page number.
        /// </summary>
        public int CurrentPageNumber { get; set; }

        /// <summary>
        /// Establishes the total record count from the derived collection source.
        /// </summary>
        public int TotalRecordCount { get; set; }

        /// <summary>
        /// Establishes the number of records to allow on a per-page basis.
        /// </summary>
        public int RecordsPerPage { get; set; }

        /// <summary>
        /// Determines the previous page number and defaults to one
        /// if the current page is the first page.
        /// </summary>
        public int? PreviousPageNumber => (CurrentPageNumber > 1) ? CurrentPageNumber - 1 : null;

        /// <summary>
        /// Determines the previous page number and defaults
        /// to the last page number if the current page is the last page.
        /// </summary>
        public int? NextPageNumber => (CurrentPageNumber < TotalNumberOfPages) ? CurrentPageNumber + 1 : null;

        /// <summary>
        /// Determines the total number of pages available for pagination.
        /// </summary>
        public int TotalNumberOfPages => GetTotalNumberOfPages();

        /// <summary>
        /// Determines whether pagination can be applied i.e. page sequence length has values.
        /// </summary>
        public bool IsPageable => TotalNumberOfPages > 1;

        /// <summary>
        /// Sets the default first page number to 1.
        /// </summary>
        public int FirstPageNumber => 1;

        /// <summary>
        /// Determines whether the current page is the last page in the sequence.
        /// </summary>
        //public bool IsLastPage => CurrentPageNumber == TotalNumberOfPages;

        public bool IsLastPage => NextPageNumber == null;

        /// <summary>
        /// Determines whether the current page is the first page in the sequence.
        /// </summary>
        public bool IsFirstPage => PreviousPageNumber == null;

        /// <summary>
        /// Determines the current page sequence (i.e. array of page numbers)
        /// based on the current page and the total number of pages available.
        /// </summary>
        public int[] CurrentPageSequence =>
            _pager.GetPageSequence(CurrentPageNumber, TotalNumberOfPages);

        /// <summary>
        /// Determines whether the current page falls within the lower paging
        /// boundary, given the total number of pages.
        /// </summary>
        public bool PageSequenceIncludesFirstPage =>
            _pager.PageSequenceIncludesFirstPage(CurrentPageNumber, TotalNumberOfPages);

        /// <summary>
        /// Determines whether the current page falls within the lower paging threshold.
        /// </summary>
        public bool HasMoreLowerPagesAvailable =>
            _pager.HasMoreLowerPagesAvailable(CurrentPageNumber, TotalNumberOfPages);

        /// <summary>
        /// Determines whether the current page falls within the upper paging
        /// boundary, given the total number of pages provisioned.
        /// </summary>
        public bool PageSequenceIncludesLastPage =>
            _pager.PageSequenceIncludesLastPage(CurrentPageNumber, TotalNumberOfPages);

        /// <summary>
        /// Determines whether the current page falls within the upper paging threshold.
        /// </summary>
        public bool HasMoreUpperPagesAvailable =>
            _pager.HasMoreUpperPagesAvailable(CurrentPageNumber, TotalNumberOfPages);

        /// <summary>
        /// Gets the total number of pages.
        /// </summary>
        /// <returns>
        /// The calculated number of pages available.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if either we have a total record count of zero, or records per-page
        /// configured to zero.
        /// </exception>
        private int GetTotalNumberOfPages()
        {
            if (TotalRecordCount == 0) {
                throw new ArgumentException("The record count must be greater than zero.");
            }

            if (RecordsPerPage == 0){
                throw new ArgumentException("The page size must be greater than zero.");
            }

            return (TotalRecordCount / RecordsPerPage) + (TotalRecordCount % RecordsPerPage > 0 ? 1 : 0);
        }
    }
}