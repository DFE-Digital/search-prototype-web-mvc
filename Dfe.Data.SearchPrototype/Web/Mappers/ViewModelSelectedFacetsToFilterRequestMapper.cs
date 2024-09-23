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
            List<FilterRequest> filterRequests = [];

            foreach (KeyValuePair<string, List<string>> filterResult in input)
            {
                filterRequests.Add(MapFromFilterRequestViewModel(filterResult));
            }

            return filterRequests;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterRequest"></param>
        /// <returns></returns>
        private static FilterRequest MapFromFilterRequestViewModel(
            KeyValuePair<string, List<string>> filterRequest) => new(filterRequest.Key, filterRequest.Value.Cast<object>().ToList());
    }
}
