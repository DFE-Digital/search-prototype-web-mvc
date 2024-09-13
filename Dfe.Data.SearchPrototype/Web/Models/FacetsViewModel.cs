namespace Dfe.Data.SearchPrototype.Web.Models
{
    public class FacetsViewModel
    {
        public List<TestFacet> TheTestFacets { get; set; }
    }

    public class TestFacet
    {
        public string Name { get; set; }
        public List<TestFacetValue> FacetValues { get; set; }
    }

    public class TestFacetValue
    {
        public string Value { get; set; }
        public bool IsSelected { get; set; }
    }
}
