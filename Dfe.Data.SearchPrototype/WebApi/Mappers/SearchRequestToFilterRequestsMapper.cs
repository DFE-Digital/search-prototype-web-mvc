using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.WebApi.Controllers;

namespace Dfe.Data.SearchPrototype.WebApi.Mappers;

public class SearchRequestToFilterRequestsMapper : IMapper<SearchRequest, IList<FilterRequest>>
{
    IList<FilterRequest> IMapper<SearchRequest, IList<FilterRequest>>.MapFrom(SearchRequest input)
    {
        var response = new List<FilterRequest>();
        if (input.EstablishmentStatus != null)
        {
            var filterRequest = new FilterRequest("ESTABLISHMENTSTATUSNAME", input.EstablishmentStatus.Select(value => (object)value).ToList());
            response.Add(filterRequest);
        };
        if (input.PhaseOfEducation != null)
        {
            var filterRequest = new FilterRequest("PHASEOFEDUCATION", input.PhaseOfEducation.Select(value => (object)value).ToList());
            response.Add(filterRequest);
        }
        return response;
    }
}
