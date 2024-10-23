using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.WebApi.Controllers;

namespace Dfe.Data.SearchPrototype.WebApi.Mappers;

public class SearchRequestToFilterRequestsMapper : IMapper<SearchRequest, IList<FilterRequest>?>
{
    IList<FilterRequest>? IMapper<SearchRequest, IList<FilterRequest>?>.MapFrom(SearchRequest input)
    {
        if (input.EstablishmentStatus == null && input.PhaseOfEducation == null)
        {
            return null;
        }
        var response = new List<FilterRequest>();
        if (input.EstablishmentStatus != null)
        {
            response.Add(
                new FilterRequest(
                    "ESTABLISHMENTSTATUSNAME",
                    input.EstablishmentStatus.Select(value => (object)value).ToList()));
        };
        if (input.PhaseOfEducation != null)
        {
            response.Add(
                new FilterRequest(
                    "PHASEOFEDUCATION",
                    input.PhaseOfEducation.Select(value => (object)value).ToList()));
        }
        return response;
    }
}
