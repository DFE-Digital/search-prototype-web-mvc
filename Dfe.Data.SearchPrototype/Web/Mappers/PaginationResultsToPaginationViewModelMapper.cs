using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared;
using Dfe.Data.SearchPrototype.Web.Options;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata.Ecma335;

namespace Dfe.Data.SearchPrototype.Web.Mappers;

/// <summary>
/// Facilitates mapping from the pagination results i.e. current page number
/// and total record count, into a <see cref="Pagination"/> instance which
/// encapsulates the pagination context and behaviour.
/// </summary>
public class PaginationResultsToPaginationViewModelMapper : IMapper<(int, int), Pagination>
{
    private readonly PaginationOptions _paginationOptions;

    /// <summary>
    /// The following dependencies include the pager behaviours used by the <see cref="Pagination"/>
    /// view-model (determines how to construct a page sequence based on the current page number,
    /// and the number of records, and derived pages available), and the pagination options used
    /// by the underlying pagination view-model, i.e. number of records per-page.
    /// </summary>
    /// <param name="paginationOptions">
    /// Provides the configuration options used by the underlying pagination
    /// view-model, i.e. number of records per-page.
    /// </param>
    public PaginationResultsToPaginationViewModelMapper(IOptions<PaginationOptions> paginationOptions)
    {
        _paginationOptions = paginationOptions.Value;
    }

    /// <summary>
    /// Provides the functionality to map from pagination results,
    /// i.e.current page number and total record count, into a <see cref="Pagination"/> view-model.
    /// The resulting <see cref="Pagination"/> instance is used to establish the baseline
    /// context on which pagination behaviour is prescribed.
    /// </summary>
    /// <param name="input">
    /// Tuple which encapsulates the current page number and total record count.
    /// </param>
    /// <returns>
    /// A configured <see cref="Pagination"/> view-model instance.
    /// </returns>
    public Pagination MapFrom((int, int) input) 
       {
        Pagination pagination = new (
            currentPageNumber: input.Item1,
            totalRecordCount: input.Item2,
            recordsPerPage: _paginationOptions.RecordsPerPage
            );
        return pagination;
    }
}
