namespace Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared
{
    /// <summary>
    /// TODO: Break this class up around structural and behavioural pagination concerns.
    /// Review naming and terminology to clean things up a bit.
    /// </summary>
    public sealed class Pagination
    {
        /// <summary>
        /// 
        /// </summary>
        public int CurrentPageNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalRecordCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int RecordsPerPage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PageSequencePaddingSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PreviousPageNumber => CurrentPageNumber - 1;

        /// <summary>
        /// 
        /// </summary>
        public int NextPageNumber => CurrentPageNumber + 1;

        /// <summary>
        /// 
        /// </summary>
        public List<int> CurrentPageSequence => GetPageSequence();

        /// <summary>
        /// 
        /// </summary>
        public int TotalNumberOfPages => GetTotalNumberOfPages();

        /// <summary>
        /// 
        /// </summary>
        public bool HasPageableResults => CurrentPageSequence.Count > 1;

        /// <summary>
        /// 
        /// </summary>
        public int FirstPageNumber => 1;

        /// <summary>
        /// 
        /// </summary>
        public bool IsLastPage => CurrentPageNumber == TotalNumberOfPages;

        /// <summary>
        /// 
        /// </summary>
        public bool IsFirstPage => CurrentPageNumber == FirstPageNumber;

        /// <summary>
        /// 
        /// </summary>
        public bool CurrentPageInsideLowerPaddingBoundary => CurrentPageSequence[0] < PageSequencePaddingSize;

        /// <summary>
        /// 
        /// </summary>
        public bool CurrentPageOnLowerPaddingBoundaryThreshold => CurrentPageSequence[0] < PageSequencePaddingSize + 1;
        
        /// <summary>
        /// 
        /// </summary>
        public bool CurrentPageOnUpperPaddingBoundaryThreshold => CurrentPageNumber > (TotalNumberOfPages - (PageSequencePaddingSize * 2));

        /// <summary>
        /// 
        /// </summary>
        public bool CurrentPageInsideUpperPaddingBoundaryThreshold => CurrentPageNumber > (TotalNumberOfPages - (PageSequencePaddingSize + 1));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<int> GetPageSequence()
        {
            int totalPageCount = GetTotalNumberOfPages();
            int[] lowerPagePadding =
                Enumerable.Range(1, PageSequencePaddingSize).ToArray();
            int[] upperPagePadding =
                Enumerable.Range(totalPageCount - PageSequencePaddingSize + 1, PageSequencePaddingSize).ToArray();

            int firstSequencePageNumber = CurrentPageNumber - PageSequencePaddingSize;
            int pageSequenceSize = (PageSequencePaddingSize * 2);
            int sequencePageCount = pageSequenceSize + 1;

            if (lowerPagePadding.Contains(CurrentPageNumber)){
                firstSequencePageNumber = 1;
                sequencePageCount =
                    (totalPageCount <= pageSequenceSize) ? totalPageCount : sequencePageCount;
            }
            else if (upperPagePadding.Contains(CurrentPageNumber)){
                firstSequencePageNumber = totalPageCount - pageSequenceSize;
            }

            return Enumerable.Range(firstSequencePageNumber, sequencePageCount).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <exception cref="ArgumentException">
        /// 
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