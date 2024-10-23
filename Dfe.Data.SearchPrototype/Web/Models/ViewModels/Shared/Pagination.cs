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
        public Dictionary<int, int[]> PageSequences { get; set; } = [];

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
        public bool IsLastPage => CurrentPageNumber == GetTotalNumberOfPages();

        public List<int> GetPageSequence()
        {
            int totalPageCount = GetTotalNumberOfPages();
            List<int> allPages = Enumerable.Range(1, totalPageCount).ToList();
            int pagePaddingSize = 2;
            int additionalPadding = totalPageCount - CurrentPageNumber;
            int startPageNumberOfPageSequence = 0;
            ///IF AVAILABLE PADDING SIZE = PAGE PADDING SIZE - ADD {PADDING SIZE} BUTTONS
            ///IF AVAILABLE PADDING SIZE < PAGE PADDING SIZE - CHECK IF WE CAN ADD IT ON THE OTHER SIDE BUT NO MORE THAN PADDING SIZE *2 LEFT OR RIGHT
            ///IF AVAIALABLE PADDING SIZE > PAGE PADDING SIZE + 1 THEN ADD ELIPSIS AND FIRST OR LAST PAGE

            //if (CurrentPageNumber == 1)
            //{

            //    startPageNumberOfPageSequence = CurrentPageNumber;
            //}
            //else if (CurrentPageNumber == 2)
            //{
            //    startPageNumberOfPageSequence = CurrentPageNumber - 1;
            //}


            // if page number is 1 or 2 then check if we can add it to the right
            if (CurrentPageNumber <= pagePaddingSize)
            {
                startPageNumberOfPageSequence = 1;
            }
            //deals with the ideal situation full padding(2) on both sides
            else if (CurrentPageNumber > pagePaddingSize || CurrentPageNumber < (totalPageCount - pagePaddingSize))
            {
                //if page number is a last number
                if (CurrentPageNumber == totalPageCount)
                {
                    startPageNumberOfPageSequence = CurrentPageNumber - (pagePaddingSize + pagePaddingSize);
                }
                //if padding on the right side is not a full padding(less than 2 in this example)
                else if (CurrentPageNumber > (totalPageCount - pagePaddingSize))
                {

                    startPageNumberOfPageSequence = CurrentPageNumber - (pagePaddingSize + additionalPadding);
                }
                else
                {
                    //pagePaddingSize number 3 >  padding size 2 then we can add full padding 2 buttons
                    startPageNumberOfPageSequence = CurrentPageNumber - pagePaddingSize;
                }
               
            }
            List<int> pageSequence = Enumerable.Range(startPageNumberOfPageSequence, (pagePaddingSize + pagePaddingSize + 1)).ToList();
            return pageSequence;
        }






        public int GetTotalNumberOfPages()
        {
            if (TotalRecordCount == 0)
                throw new ArgumentException("The record count must be greater than zero.");

            if (RecordsPerPage == 0)
                throw new ArgumentException("The page size must be greater than zero.");

            return TotalRecordCount / RecordsPerPage + (TotalRecordCount % RecordsPerPage > 0 ? 1 : 0);
        }
    }
}