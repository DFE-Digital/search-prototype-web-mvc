namespace Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PaginationSequencer : IPaginationSequencer
    {
        /// <summary>
        /// 
        /// </summary>
        private const int PageSequencePaddingSize = 2;

        /// <summary>
        /// 
        /// </summary>
        public bool IsCurrentPageInsideLowerPaddingBoundary(
            int currentPageNumber,
            int totalNumberOfPages) =>
                GetPageSequence(currentPageNumber, totalNumberOfPages)?[0] < PageSequencePaddingSize;

        /// <summary>
        /// 
        /// </summary>
        public bool IsCurrentPageOnLowerPaddingBoundaryThreshold(
            int currentPageNumber,
            int totalNumberOfPages) =>
                GetPageSequence(currentPageNumber, totalNumberOfPages)?[0] < PageSequencePaddingSize + 1;

        /// <summary>
        /// 
        /// </summary>
        public bool IsCurrentPageInsideUpperPaddingBoundary(
            int currentPageNumber,
            int totalNumberOfPages) =>
                currentPageNumber > (totalNumberOfPages - (PageSequencePaddingSize + 1));

        /// <summary>
        /// 
        /// </summary>
        public bool IsCurrentPageOnUpperPaddingBoundaryThreshold(
            int currentPageNumber,
            int totalNumberOfPages) =>
                currentPageNumber > (totalNumberOfPages - (PageSequencePaddingSize * 2));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPageNumber"></param>
        /// <param name="totalNumberOfPages"></param>
        /// <returns></returns>
        public List<int> GetPageSequence(int currentPageNumber, int totalNumberOfPages)
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

            return Enumerable.Range(firstSequencePageNumber, sequencePageCount).ToList();
        }
    }
}
