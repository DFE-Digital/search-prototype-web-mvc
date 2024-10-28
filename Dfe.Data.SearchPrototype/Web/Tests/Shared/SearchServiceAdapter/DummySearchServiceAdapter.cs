using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.ServiceAdapters;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.SearchServiceAdapter.Resources;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.SearchServiceAdapter
{
    public sealed class DummySearchServiceAdapter<TSearchResult> : ISearchServiceAdapter where TSearchResult : class
    {
        private readonly IJsonFileLoader _jsonFileLoader;

        public DummySearchServiceAdapter(IJsonFileLoader jsonFileLoader)
        {
            _jsonFileLoader = jsonFileLoader;
        }

        public async Task<SearchResults> SearchAsync(SearchServiceAdapterRequest searchServiceAdapterRequest)
        {
            string json = await _jsonFileLoader.LoadJsonFile();

            JObject establishmentsObject = JObject.Parse(json);

            IEnumerable<Establishment> allEstablishments =
                from establishmentToken in establishmentsObject["establishments"]
                select new Establishment(
                    (string)establishmentToken["urn"]!,
                    (string)establishmentToken["name"]!,
                    new Address(
                        (string)establishmentToken["address"]!["street"]!,
                        (string)establishmentToken["address"]!["locality"]!,
                        (string)establishmentToken["address"]!["address3"]!,
                        (string)establishmentToken["address"]!["town"]!,
                        (string)establishmentToken["address"]!["postcode"]!),
                    (string)establishmentToken["establishmentType"]!,
                    (string)establishmentToken["phaseOfEducation"]!,
                    (string)establishmentToken["establishmentStatusName"]!);

            var establishments = allEstablishments
                .Where( establishment => establishment.Name.Contains(searchServiceAdapterRequest.SearchKeyword))
                .ToList();

            return new SearchResults()
            {
                Establishments =
                    new EstablishmentResults(
                        establishments, totalNumberOfEstablishments: establishments.Count)
            };
        }
    }
}
