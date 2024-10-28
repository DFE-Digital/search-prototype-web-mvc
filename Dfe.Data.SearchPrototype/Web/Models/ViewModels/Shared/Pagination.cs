namespace Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared
{
    /// <summary>
    ///
    /// </summary>
    public sealed class Pagination
    {
        private readonly IPager _pager;

        /// <summary>
        /// 
        /// </summary>
        public Pagination(IPager pager) => _pager = pager;

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
        public int PreviousPageNumber => CurrentPageNumber - 1;

        /// <summary>
        /// 
        /// </summary>
        public int NextPageNumber => CurrentPageNumber + 1;

        /// <summary>
        /// 
        /// </summary>
        public List<int> CurrentPageSequence =>
            _pager.GetPageSequence(CurrentPageNumber, TotalNumberOfPages);

        /// <summary>
        /// 
        /// </summary>
        public int TotalNumberOfPages => GetTotalNumberOfPages();

        /// <summary>
        /// 
        /// </summary>
        public bool IsPageable => CurrentPageSequence.Count > 1;

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
        public bool CurrentPageInLowerPagingBoundary =>
            _pager.IsCurrentPageInLowerPagingBoundary(CurrentPageNumber, TotalNumberOfPages);

        /// <summary>
        /// 
        /// </summary>
        public bool CurrentPageOnLowerPagingThreshold =>
            _pager.IsCurrentPageOnLowerPagingThreshold(CurrentPageNumber, TotalNumberOfPages);

        /// <summary>
        /// 
        /// </summary>
        public bool CurrentPageInUpperPagingBoundary =>
            _pager.IsCurrentPageInUpperPagingBoundary(CurrentPageNumber, TotalNumberOfPages);

        /// <summary>
        /// 
        /// </summary>
        public bool CurrentPageOnUpperPagingThreshold =>
            _pager.IsCurrentPageOnUpperPagingThreshold(CurrentPageNumber, TotalNumberOfPages);

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