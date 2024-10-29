namespace Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared
{
    public interface IPager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPageNumber"></param>
        /// <param name="totalNumberOfPages"></param>
        /// <returns></returns>
        public bool IsCurrentPageInLowerPagingBoundary(int currentPageNumber, int totalNumberOfPages);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPageNumber"></param>
        /// <param name="totalNumberOfPages"></param>
        /// <returns></returns>
        public bool IsCurrentPageOnLowerPagingThreshold(int currentPageNumber, int totalNumberOfPages);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPageNumber"></param>
        /// <param name="totalNumberOfPages"></param>
        /// <returns></returns>
        public bool IsCurrentPageInUpperPagingBoundary(int currentPageNumber, int totalNumberOfPages);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPageNumber"></param>
        /// <param name="totalNumberOfPages"></param>
        /// <returns></returns>
        public bool IsCurrentPageOnUpperPagingThreshold(int currentPageNumber, int totalNumberOfPages);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPageNumber"></param>
        /// <param name="totalNumberOfPages"></param>
        /// <returns></returns>
        int[] GetPageSequence(int currentPageNumber, int totalNumberOfPages);
    }
}
