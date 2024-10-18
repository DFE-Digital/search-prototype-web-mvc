using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.WebApi.Controllers;

namespace Dfe.Data.SearchPrototype.WebApi.Mappers;

public class SearchRequestToFilterRequestsMapper : IMapper<SearchRequest, IList<FilterRequest>>
{
    IList<FilterRequest> IMapper<SearchRequest, IList<FilterRequest>>.MapFrom(SearchRequest input)
    {
        throw new NotImplementedException();
    }
}
