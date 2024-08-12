using Dfe.Data.SearchPrototype.SearchForEstablishments;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.SearchServiceAdapter.Resources;
using Newtonsoft.Json.Linq;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.SearchServiceAdapter
{
    public sealed class DummySearchServiceAdapter<TSearchResult> : ISearchServiceAdapter where TSearchResult : class
    {
        private readonly IJsonFileLoader _jsonFileLoader;

        public DummySearchServiceAdapter(IJsonFileLoader jsonFileLoader)
        {
            _jsonFileLoader = jsonFileLoader;
        }

        public async Task<EstablishmentResults> SearchAsync(SearchContext searchContext)
        {
            string json = await _jsonFileLoader.LoadJsonFile();

            JObject establishmentsObject = JObject.Parse(json);

            IEnumerable<Establishment> establishments =
                from establishmentToken in establishmentsObject["establishments"]
                where establishmentToken["name"]!.ToString().Contains(searchContext.SearchKeyword)
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
                    new EducationPhase(
                        (string)establishmentToken["educationPhase"]!["isPrimary"]!, 
                        (string)establishmentToken["educationPhase"]!["isSecondary"]!,
                        (string)establishmentToken["educationPhase"]!["isPost16"]!),
                    establishmentStatusCode : StatusCode.Closed)  ;

            return new EstablishmentResults(establishments);
        }
    }
}
