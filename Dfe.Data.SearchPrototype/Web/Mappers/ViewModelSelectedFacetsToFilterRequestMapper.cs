using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;

namespace Dfe.Data.SearchPrototype.Web.Mappers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ViewModelSelectedFacetsToFilterRequestMapper : IMapper<Dictionary<string, List<string>>, IList<FilterRequest>>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public IList<FilterRequest> MapFrom(Dictionary<string, List<string>> input)
        {
            List<FilterRequest> filterRequest = [];

            foreach (var item in input)
            {
                filterRequest.Add(new FilterRequest(item.Key, item.Value.ToList<object>()));
            }

            return filterRequest;
        }
    }
}
