using Bogus;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.TestDoubles
{
    public static class FacetsSelectedViewModelTestDouble
    {
        public static string GetFacetKeyFake() => new Faker().Company.CompanyName();
        public static string GetFacetValueFake() => new Faker().Company.CompanyName();

        public static Dictionary<string, List<string>> Create()
        {
            var facetsSelected = new Dictionary<string, List<string>>();
            var facetValues =
                Enumerable.Range(1, new Faker().Random.Int(1, 4))
                    .Select(_ => GetFacetValueFake() );

            Enumerable.Range(1, new Faker().Random.Int(1, 4)).ToList()
                .ForEach(_ => facetsSelected.Add(GetFacetKeyFake(), facetValues.ToList()));

            return facetsSelected;
        }
    }
}