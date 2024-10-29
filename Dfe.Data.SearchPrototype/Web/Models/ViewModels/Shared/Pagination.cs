namespace Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared
{
    /// <summary>
    ///
    /// </summary>
    public sealed class Pagination(IPager pager)
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
        public int PreviousPageNumber => (CurrentPageNumber > 0) ? CurrentPageNumber - 1 : 0;

        /// <summary>
        /// 
        /// </summary>
        public int NextPageNumber => (CurrentPageNumber < TotalNumberOfPages) ? CurrentPageNumber + 1 : TotalNumberOfPages;

        /// <summary>
        /// 
        /// </summary>
        public int[] CurrentPageSequence => pager.GetPageSequence(CurrentPageNumber, TotalNumberOfPages);

        /// <summary>
        /// 
        /// </summary>
        public int TotalNumberOfPages => GetTotalNumberOfPages();

        /// <summary>
        /// 
        /// </summary>
        public bool IsPageable => CurrentPageSequence.Length > 1;

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
        public bool CurrentPageInLowerPagingBoundary => pager.IsCurrentPageInLowerPagingBoundary(CurrentPageNumber, TotalNumberOfPages);

        /// <summary>
        /// 
        /// </summary>
        public bool CurrentPageInLowerPagingThreshold => pager.IsCurrentPageInLowerPagingThreshold(CurrentPageNumber, TotalNumberOfPages);

        /// <summary>
        /// 
        /// </summary>
        public bool CurrentPageInUpperPagingBoundary => pager.IsCurrentPageInUpperPagingBoundary(CurrentPageNumber, TotalNumberOfPages);

        /// <summary>
        /// 
        /// </summary>
        public bool CurrentPageInUpperPagingThreshold => pager.IsCurrentPageInUpperPagingThreshold(CurrentPageNumber, TotalNumberOfPages);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
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