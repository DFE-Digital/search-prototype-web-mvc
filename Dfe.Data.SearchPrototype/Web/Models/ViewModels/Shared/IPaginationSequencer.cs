namespace Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared
{
    public interface IPaginationSequencer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPageNumber"></param>
        /// <param name="totalNumberOfPages"></param>
        /// <returns></returns>
        public bool IsCurrentPageInsideLowerPaddingBoundary(int currentPageNumber, int totalNumberOfPages);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPageNumber"></param>
        /// <param name="totalNumberOfPages"></param>
        /// <returns></returns>
        public bool IsCurrentPageOnLowerPaddingBoundaryThreshold(int currentPageNumber, int totalNumberOfPages);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPageNumber"></param>
        /// <param name="totalNumberOfPages"></param>
        /// <returns></returns>
        public bool IsCurrentPageInsideUpperPaddingBoundary(int currentPageNumber, int totalNumberOfPages);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPageNumber"></param>
        /// <param name="totalNumberOfPages"></param>
        /// <returns></returns>
        public bool IsCurrentPageOnUpperPaddingBoundaryThreshold(int currentPageNumber, int totalNumberOfPages);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPageNumber"></param>
        /// <param name="totalNumberOfPages"></param>
        /// <returns></returns>
        List<int> GetPageSequence(int currentPageNumber, int totalNumberOfPages);
    }
}
