namespace Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared
{
    /// <summary>
    /// TODO: Break this class up around structural and behavioural pagination concerns.
    /// Review naming and terminology to clean things up a bit.
    /// </summary>
    public sealed class Pagination
    {
        private readonly IPaginationSequencer _paginationSequencer;

        /// <summary>
        /// 
        /// </summary>
        public Pagination(IPaginationSequencer paginationSequencer)
        {
            _paginationSequencer = paginationSequencer;
        }

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
            _paginationSequencer.GetPageSequence(CurrentPageNumber, TotalNumberOfPages);

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
        public bool CurrentPageInsideLowerPaddingBoundary =>
            _paginationSequencer
                .IsCurrentPageInsideLowerPaddingBoundary(CurrentPageNumber, TotalNumberOfPages);

        /// <summary>
        /// 
        /// </summary>
        public bool CurrentPageOnLowerPaddingBoundaryThreshold =>
            _paginationSequencer
                .IsCurrentPageOnLowerPaddingBoundaryThreshold(CurrentPageNumber, TotalNumberOfPages);
        
        /// <summary>
        /// 
        /// </summary>
        public bool CurrentPageOnUpperPaddingBoundaryThreshold =>
            _paginationSequencer
                .IsCurrentPageOnUpperPaddingBoundaryThreshold(CurrentPageNumber, TotalNumberOfPages);

        /// <summary>
        /// 
        /// </summary>
        public bool CurrentPageInsideUpperPaddingBoundary =>
            _paginationSequencer
                .IsCurrentPageInsideUpperPaddingBoundary(CurrentPageNumber, TotalNumberOfPages);

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