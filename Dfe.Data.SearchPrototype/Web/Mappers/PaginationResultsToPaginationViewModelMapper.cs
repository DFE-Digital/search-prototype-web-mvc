using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared;

namespace Dfe.Data.SearchPrototype.Web.Mappers;

/// <summary>
/// 
/// </summary>
public class PaginationResultsToPaginationViewModelMapper : IMapper<(int, int), Pagination>
{
    private readonly IPager _paginationSequencer;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="paginationSequencer">
    /// 
    /// </param>
    public PaginationResultsToPaginationViewModelMapper(IPager paginationSequencer)
    {
        _paginationSequencer = paginationSequencer;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Pagination MapFrom((int, int) input) =>
        new(_paginationSequencer)
        {
            CurrentPageNumber = input.Item1,
            TotalRecordCount = input.Item2,
            RecordsPerPage = 10 // from config?
        };
}
