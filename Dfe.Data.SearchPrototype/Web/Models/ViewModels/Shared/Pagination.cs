namespace Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared
{
    /// <summary>
    /// View model to describe and assign pagination related presentation concerns.
    /// </summary>
    public sealed class Pagination
    {
        private int[] _currentPageSequence;

        private int _totalNumberOfPages;

        private const int PageSequencePaddingSize = 2;

        public int[] CurrentPageSequence => _currentPageSequence;
        public int TotalNumberOfPages => _totalNumberOfPages;

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
        /// Sets the default first page number to 1.
        /// </summary>
        public int FirstPageNumber => 1;

        /// <summary>
        /// Determines whether pagination can be applied i.e. page sequence length has values.
        /// </summary>
        public bool IsPageable => _totalNumberOfPages > 1;

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
        /// Determines the previous page number and defaults to one
        /// if the current page is the first page.
        /// </summary>
        public int? PreviousPageNumber => (CurrentPageNumber > 1) ? CurrentPageNumber - 1 : null;

        /// <summary>
        /// Determines the previous page number and defaults
        /// to the last page number if the current page is the last page.
        /// </summary>
        public int? NextPageNumber => (CurrentPageNumber < _totalNumberOfPages) ? CurrentPageNumber + 1 : null;


        /// <summary>
        /// Determines if FirstPage button should be displayed
        /// </summary>
        /// <returns>
        /// True or false based on whether the condition is met.
        /// </returns>
        public bool PageSequenceIncludesFirstPage =>
               _currentPageSequence[0] == 1;

        /// <summary>
        /// Determines if the front ellipsis should be displayed.
        /// </summary>
        /// <returns>
        /// True or false based on whether the condition is met.
        /// </returns>
        public bool HasMoreLowerPagesAvailable =>
                !(_currentPageSequence[0] < PageSequencePaddingSize + 1);
        /// <summary>
        /// Determines if LastPage button should be displayed.
        /// </summary>
        /// <returns>
        /// True or false based on whether the condition is met
        /// </returns>

        public bool PageSequenceIncludesLastPage =>
                _currentPageSequence.Contains(TotalNumberOfPages);

        /// <summary>
        /// Determines if the end ellipsis should be displayed.
        /// </summary>
        /// <param name="CurrentPageNumber">
        /// The current page selected.
        /// </param>
        /// <param name="TotalNumberOfPages">
        /// The total number of pages available.
        /// </param>
        /// <returns>
        /// True or false based on whether the condition is met.
        /// </returns>
        public bool HasMoreUpperPagesAvailable =>
                (CurrentPageNumber <= (TotalNumberOfPages - (PageSequencePaddingSize * 2))) &&
                (TotalNumberOfPages != ((PageSequencePaddingSize * 2) + 1));

        public Pagination(int currentPageNumber, int totalRecordCount, int recordsPerPage)
        {
            CurrentPageNumber = currentPageNumber;
            TotalRecordCount = totalRecordCount;
            RecordsPerPage = recordsPerPage;

            _totalNumberOfPages = GetTotalNumberOfPages();
            _currentPageSequence = GetPageSequence(CurrentPageNumber, _totalNumberOfPages);
        }

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
            if (RecordsPerPage == 0){
                return 0;
            }

            return (TotalRecordCount / RecordsPerPage) + (TotalRecordCount % RecordsPerPage > 0 ? 1 : 0);
        }
        /// <summary>
        /// Determines the current page sequence to return given the current page number and total number of pages.
        /// This method ensures the page selected is maintained in the middle of the sequence if it does not fall on,
        /// or within the lower or upper bounds, thus giving the impression that the pages are scrolling forwards or
        /// backwards depending on the selection made. If the page falls on or within the lower or upper bounds,
        /// the page sequence is adjusted accordingly to ensure the current page number is assigned to the correct sequence ordering.
        /// </summary>
        /// <param name="currentPageNumber">
        /// The current page selected.
        /// </param>
        /// <param name="totalNumberOfPages">
        /// The total number of pages available.
        /// </param>
        /// <returns>
        /// An integer array which represents the pages to be contained within a given pagination sequence.
        /// </returns>
        private int[] GetPageSequence(int currentPageNumber, int totalNumberOfPages)
        {
            int[] lowerPagePadding =
                Enumerable.Range(1, PageSequencePaddingSize).ToArray();
            int[] upperPagePadding =
                Enumerable.Range(totalNumberOfPages - PageSequencePaddingSize + 1, PageSequencePaddingSize).ToArray();

            const int pageSequenceSize = (PageSequencePaddingSize * 2);
            int firstSequencePageNumber = currentPageNumber - PageSequencePaddingSize;
            int sequencePageCount = pageSequenceSize + 1;

            if (lowerPagePadding.Contains(currentPageNumber))
            {
                firstSequencePageNumber = 1;
                sequencePageCount =
                    (totalNumberOfPages <= pageSequenceSize) ? totalNumberOfPages : sequencePageCount;
            }
            else if (upperPagePadding.Contains(currentPageNumber))
            {
                firstSequencePageNumber = totalNumberOfPages - pageSequenceSize;
            }

            return Enumerable.Range(firstSequencePageNumber, sequencePageCount).ToArray();
        }
    }

    
}