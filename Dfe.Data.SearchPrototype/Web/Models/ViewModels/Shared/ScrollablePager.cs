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
        /// Used to determine if FirstPageNumber should be displayed ahead of PageSequence
        /// Checks if the first page of PageSequence is less than pageSequencePaddingSize(2)
        /// if < 2 example: 1 2 3 4 5 [no FirstPageNumber, PageSequence (5 buttons, starting with < 2 so already covered FirstPage)
        /// if == 2 example: 1 2 3 4 5 6 [FirstPageNumber and PageSequence(5 buttons, starting with == 2)]
        /// if > 2 example: 1 ... 5 6 7 8 9 [FirstPageNumber, Ellipsis, PageSequence(5buttons, starting with > 2)].
        /// </summary>
        /// <param name="currentPageNumber">
        /// The current page selected.
        /// </param>
        /// <param name="totalNumberOfPages">
        /// The total number of pages available.
        /// </param>
        /// <returns>
        /// True or false based on whether the condition is met (i.e. the
        /// current page falls within the lower paging bounds).
        /// </returns>
        public bool IsCurrentPageInLowerPagingBoundary(
            int currentPageNumber, int totalNumberOfPages) =>
                GetFirstPageInSequence(currentPageNumber, totalNumberOfPages) < PageSequencePaddingSize;

        /// <summary>
        /// Used to determine if Ellipsis should be displayed in front of the PageSequence
        /// Essentially if between FirstPageNumber and first number of PageSequence is > 0 the ellipsis would show
        /// suggesting there is more "hidden"  pages in between.
        /// Checks if the first page of page sequence is less than pageSequencePaddingSize(2) + 1
        /// if < 3 example: 1 2 3 4 5 6 [checks for FirstPageNumber as above, NO ELLIPSIS, PageSequence]
        /// if ==  3 || > 3 example: 1 ... 3 4 5 6 7 or: 1 ... 6 7 8 9 10
        /// </summary>
        /// <param name="currentPageNumber">
        /// The current page selected.
        /// </param>
        /// <param name="totalNumberOfPages">
        /// The total number of pages available.
        /// </param>
        /// <returns>
        /// True or false based on whether the condition is met (i.e. the
        /// current page falls in the lower paging threshold).
        /// </returns>
        public bool IsCurrentPageInLowerPagingThreshold(
            int currentPageNumber, int totalNumberOfPages) =>
                GetFirstPageInSequence(currentPageNumber, totalNumberOfPages) < PageSequencePaddingSize + 1;

        /// <summary>
        /// Used to determine if LastPageNumber should be displayed following PageSequence
        /// First checks if the CurrentPgeNumber is > totalNumberOfPages - pageSequencePaddingSize(2) + 1
        /// examples:
        /// currentPageNumber 66 > 68-3:   64 65 66 67 68 [this is just PageSequence(5 buttons with last number == totalNumberOfPages) 
        /// no need to add LastPageNumber again]
        /// currentPageNumber 65 == 68-3:  63 64 65 66 67 68 [PageSequence (5 buttons) followed by LastPageNumber]
        /// currentPageNumber 64 < 68-3:   62 63 64 65 66 ... 68 [PageSequence, ellipsis, LastPageNumber]
        /// Esentially if there is 3 or more pages available after CurrentPageNumber, the lastPageNumber will be displayed
        /// PageSequence covers 2 pages following current page so we only need to add last page if more than 2
        /// Second checks if totalNumberOfPages == 5
        /// example:
        /// total number of pages == 5 : 1 2 3 4 5 no need for last page to be displayed again (5)
        /// Essentially since our PageSequence is 5, we know all pages would already be displayed.
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
        public bool IsCurrentPageInUpperPagingBoundary(
            int currentPageNumber, int totalNumberOfPages) =>
                (currentPageNumber > (totalNumberOfPages - (PageSequencePaddingSize + 1))) ||
                (totalNumberOfPages == ((PageSequencePaddingSize * 2) + 1));

        /// <summary>
        /// Used to determine if ellipsis should be displayed following PageSequence
        /// Checks if CurrentPageNumber > TotalNumberOfPages - 4
        /// examples:
        /// currentPageNumber 66 > 68-4:   64 65 66 67 68 
        /// [this is just PageSequence, no need for ellipsis 
        /// as there is no hidden page numbers between LastPageNumber and last page within PageSequence (67 and 68)]
        /// currentPageNumber 64 == 68-4:  62 63 64 65 66 ... 68 
        /// [PageSequence (5 buttons) Ellipsis to suggest missing pages between 66 and 68, followed by LastPageNumber]
        /// currentPageNumber 63 < 68-4:   61 62 63 64 65 ... 68 [PageSequence, Ellipsis to suggest missing pages between 65 and 68, LastPageNumber]
        /// Then checks if TotalNumberOfPages == 5 (Our PageSequence length)
        /// as our pageSequence would cover all pages 1 2 3 4 5 we would not have to add anything after that.
        /// </summary>
        /// <param name="currentPageNumber">
        /// The current page selected.
        /// </param>
        /// <param name="totalNumberOfPages">
        /// The total number of pages available.
        /// </param>
        /// <returns>
        /// True or false based on whether the condition is met (i.e. the
        /// current page falls on the upper paging threshold).
        /// </returns>
        public bool IsCurrentPageInUpperPagingThreshold(
            int currentPageNumber, int totalNumberOfPages) =>
                (currentPageNumber > (totalNumberOfPages - (PageSequencePaddingSize * 2))) ||
                (totalNumberOfPages == ((PageSequencePaddingSize * 2) + 1));

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
        /// The current page selected through the pagination provisioned.
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
