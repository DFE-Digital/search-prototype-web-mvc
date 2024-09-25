using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;

namespace Dfe.Data.SearchPrototype.Web.Mappers
{
    /// <summary>
    /// Facilitates mapping from the received dictionary of selected facets,
    /// into the required list of <see cref="FilterRequest"/> instances.
    /// </summary>
    public sealed class SelectedFacetsToFilterRequestsMapper : IMapper<Dictionary<string, List<string>>, IList<FilterRequest>>
    {
        /// <summary>
        /// Provides the functionality to map from user selected facets in the form of a
        /// dictionary, to a configured list of <see cref="FilterRequest"/> instances.
        /// </summary>
        /// <param name="input">
        /// The dictionary of selected facets (user input).
        /// </param>
        /// <returns>
        /// The configured list of <see cref="FilterRequest"/> instances expected.
        /// </returns>
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
        /// Provides mapping from a single facet (key-value-pair dictionary entry)
        /// to a configured <see cref="FilterRequest"/> instance.
        /// </summary>
        /// <param name="filterRequest">
        /// The single facet (key-value-pair dictionary entry).
        /// </param>
        /// <returns>
        /// The configured <see cref="FilterRequest"/> instance expected.
        /// </returns>
        private static FilterRequest MapFromFilterRequestViewModel(
            KeyValuePair<string, List<string>> filterRequest) =>
                new(filterRequest.Key, filterRequest.Value.Cast<object>().ToList());
    }
}
