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
        /// Determines if FirstPage button should be displayed
        /// </summary>
        /// <param name="currentPageNumber">
        /// The current page selected.
        /// </param>
        /// <param name="totalNumberOfPages">
        /// The total number of pages available.
        /// </param>
        /// <returns>
        /// True or false based on whether the condition is met.
        /// </returns>
        public bool PageSequenceIncludesFirstPage(
            int currentPageNumber, int totalNumberOfPages) =>
                GetFirstPageInSequence(currentPageNumber, totalNumberOfPages) == 1;

        /// <summary>
        /// Determines if the front ellipsis should be displayed.
        /// </summary>
        /// <param name="currentPageNumber">
        /// The current page selected.
        /// </param>
        /// <param name="totalNumberOfPages">
        /// The total number of pages available.
        /// </param>
        /// <returns>
        /// True or false based on whether the condition is met.
        /// </returns>
        public bool HasMoreLowerPagesAvailable(
            int currentPageNumber, int totalNumberOfPages) =>
                !(GetFirstPageInSequence(currentPageNumber, totalNumberOfPages) < PageSequencePaddingSize + 1);

        /// <summary>
        /// Determines if LastPage button should be displayed.
        /// </summary>
        /// <param name="currentPageNumber">
        /// The current page selected.
        /// </param>
        /// <param name="totalNumberOfPages">
        /// The total number of pages available.
        /// </param>
        /// <returns>
        /// True or false based on whether the condition is met (i.e. the
        /// current page falls within the upper paging bounds).
        /// </returns>
        /// // TODO CML method doesn't describe the logic used
        public bool PageSequenceIncludesLastPage(
            int currentPageNumber, int totalNumberOfPages) =>
                (currentPageNumber > (totalNumberOfPages - (PageSequencePaddingSize + 1))) ||
                (totalNumberOfPages == ((PageSequencePaddingSize * 2) + 1));

        /// <summary>
        /// Determines if the end ellipsis should be displayed.
        /// </summary>
        /// <param name="currentPageNumber">
        /// The current page selected.
        /// </param>
        /// <param name="totalNumberOfPages">
        /// The total number of pages available.
        /// </param>
        /// <returns>
        /// True or false based on whether the condition is met.
        /// </returns>
        /// TODo CML - redo logic
        public bool HasMoreUpperPagesAvailable(
            int currentPageNumber, int totalNumberOfPages) =>
                !((currentPageNumber > (totalNumberOfPages - (PageSequencePaddingSize * 2))) ||
                (totalNumberOfPages == ((PageSequencePaddingSize * 2) + 1)));

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
        /// The current page selected.
        /// </param>
        /// <param name="totalNumberOfPages">
        /// The total number of pages available.
        /// </param>
        /// <returns>
        /// The first page available within the given pagination sequence.
        /// </returns>
        private int GetFirstPageInSequence(int currentPageNumber, int totalNumberOfPages) => GetPageSequence(currentPageNumber, totalNumberOfPages)[0];
    }
}
