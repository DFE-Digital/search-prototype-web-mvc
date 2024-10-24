namespace Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared
{
    /// <summary>
    /// 
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
        /// <param name="pagePaddingSize">
        /// 
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public List<int> GetPageSequenceWithBounds()
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
        public int GetTotalNumberOfPages()
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