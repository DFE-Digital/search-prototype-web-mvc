namespace Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared
{
    /// <summary>
    /// Provides functionality to allow paging to work in a scrollable manner whereby moving forwards or
    /// backwards across a given page sequence means the page selected is maintained in the middle of the sequence,
    /// thus giving the impression that the pages are scrolling forwards or backwards depending on the selection made.
    /// </summary>
    public sealed class ScrollablePager : IPager
    {
        /// <summary>
        /// Sets the default number of lower and upper pages to 2. This means that
        /// if the current page falls within this lower or upper page band, certain
        /// constraints can be triggered (e.g. display/hide next previous buttons).
        /// </summary>
        private const int PageSequencePaddingSize = 2;

        /// <summary>
        /// Determines whether the current page falls within the lower paging boundary,
        /// i.e. less than 2 (given the constrained page sequence padding size).
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
        public bool IsCurrentPageInLowerPagingBoundary(
            int currentPageNumber, int totalNumberOfPages) =>
                GetFirstPageInSequence(currentPageNumber, totalNumberOfPages) < PageSequencePaddingSize;

        /// <summary>
        /// Determines whether the current page falls on the lower paging boundary,
        /// i.e. is equal to 2 (given the constrained page sequence padding size).
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
        /// current page falls on the lower paging bounds).
        /// </returns>
        public bool IsCurrentPageOnLowerPagingThreshold(
            int currentPageNumber, int totalNumberOfPages) =>
                GetFirstPageInSequence(currentPageNumber, totalNumberOfPages) < PageSequencePaddingSize + 1;

        /// <summary>
        /// Determines whether the current page falls within the upper paging boundary,
        /// i.e. less than total number of pages - 2 (given the constrained page sequence padding size).
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
        public bool IsCurrentPageInUpperPagingBoundary(
            int currentPageNumber, int totalNumberOfPages) =>
                currentPageNumber > (totalNumberOfPages - (PageSequencePaddingSize + 1));

        /// <summary>
        /// Determines whether the current page falls on the upper paging boundary,
        /// i.e. is equal to the total number of pages - 2 (given the constrained page sequence padding size).
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
        /// current page falls on the upper paging bounds).
        /// </returns>
        public bool IsCurrentPageOnUpperPagingThreshold(
            int currentPageNumber, int totalNumberOfPages) =>
                currentPageNumber > (totalNumberOfPages - (PageSequencePaddingSize * 2));

        /// <summary>
        /// Determines the current page sequence to return given the current page number and total number of pages.
        /// This method ensures the page selected is maintained in the middle of the sequence if it does not fall on,
        /// or within the lower or upper bounds, thus giving the impression that the pages are scrolling forwards or
        /// backwards depending on the selection made. If the page falls on or within the lower or upper bounds,
        /// the page sequence is adjusted accordingly to ensure the current page number is assigned to the correct sequence ordering.
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
        public int[] GetPageSequence(int currentPageNumber, int totalNumberOfPages)
        {
            int[] lowerPagePadding =
                Enumerable.Range(1, PageSequencePaddingSize).ToArray();
            int[] upperPagePadding =
                Enumerable.Range(totalNumberOfPages - PageSequencePaddingSize + 1, PageSequencePaddingSize).ToArray();

            const int pageSequenceSize = (PageSequencePaddingSize * 2);
            int firstSequencePageNumber = currentPageNumber - PageSequencePaddingSize;
            int sequencePageCount = pageSequenceSize + 1;

            if (lowerPagePadding.Contains(currentPageNumber)){
                firstSequencePageNumber = 1;
                sequencePageCount =
                    (totalNumberOfPages <= pageSequenceSize) ? totalNumberOfPages : sequencePageCount;
            }
            else if (upperPagePadding.Contains(currentPageNumber)){
                firstSequencePageNumber = totalNumberOfPages - pageSequenceSize;
            }

            return Enumerable.Range(firstSequencePageNumber, sequencePageCount).ToArray();
        }

        /// <summary>
        /// Determines the first page available within a given page sequence.
        /// </summary>
        /// <param name="currentPageNumber">
        /// The current page selected through the pagination provisioned.
        /// </param>
        /// <param name="totalNumberOfPages">
        /// The total number of pages available based on the total number of records
        /// and the page sequence width.
        /// </param>
        /// <returns>
        /// The first page available within the given pagination sequence.
        /// </returns>
        private int GetFirstPageInSequence(int currentPageNumber, int totalNumberOfPages) => GetPageSequence(currentPageNumber, totalNumberOfPages)[0];
    }
}
