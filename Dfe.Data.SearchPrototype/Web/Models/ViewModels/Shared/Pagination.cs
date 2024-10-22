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
        public int TotalPageCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<int, int[]> PageSequences { get; set; } = [];

        /// <summary>
        /// 
        /// </summary>
        public int TotalRecordCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PageSequenceWidth { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int[] CurrentPageSequence => PageSequences[CurrentPageNumber]; // throws KeyNotFoundException by default.

        /// <summary>
        /// 
        /// </summary>
        public bool IsFirstPageSequence =>
            PageSequences == null || !PageSequences.Any() ?
                throw new InvalidOperationException(
                    "Cannot determine first page result when page sequence is null or empty.") :
                PageSequences.First().Value.Contains(CurrentPageNumber);

        /// <summary>
        /// 
        /// </summary>
        public bool IsLastPageSequence =>
            PageSequences == null || !PageSequences.Any() ?
                throw new InvalidOperationException(
                    "Cannot determine last page result when page sequence is null or empty.") :
                PageSequences.Last().Value.Contains(CurrentPageNumber);

        /// <summary>
        /// 
        /// </summary>
        public bool IsFirstPage => CurrentPageNumber == 1;

        /// <summary>
        /// 
        /// </summary>
        public bool IsLastPage => CurrentPageNumber == TotalPageCount;
    }
}