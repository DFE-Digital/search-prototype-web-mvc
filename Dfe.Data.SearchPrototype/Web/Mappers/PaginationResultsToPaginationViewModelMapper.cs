using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared;

namespace Dfe.Data.SearchPrototype.Web.Mappers;

public class PaginationResultsToPaginationViewModelMapper : IMapper<(int, int), Pagination>
{
    private readonly IPaginationSequencer _paginationSequencer;

    public PaginationResultsToPaginationViewModelMapper(IPaginationSequencer paginationSequencer)
    {
        _paginationSequencer = paginationSequencer;
    }

    public Pagination MapFrom((int, int) input)
    {
        return new Pagination(_paginationSequencer)
        {
            CurrentPageNumber = input.Item1,
            TotalRecordCount = input.Item2,
            RecordsPerPage = 10 // from config?
        };
    }
}
