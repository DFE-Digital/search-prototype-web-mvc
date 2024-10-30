namespace Dfe.Data.SearchPrototype.Web.Options
{
    /// <summary>
    /// The search options to use by the <see cref="Pagination"/>
    /// which is set using the IOptions interface.
    /// </summary>
    public sealed class PaginationOptions
    {
        /// <summary>
        /// Establishes the number of records per-page,
        /// used to determine total number of pages available.
        /// </summary>
        public int RecordsPerPage { get; set; }
    }
}
