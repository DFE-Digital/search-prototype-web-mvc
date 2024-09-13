namespace Dfe.Data.SearchPrototype.Web.Models
{
    public class FacetsViewModel
    {
        public List<TestFacet> TestFacets { get; set; }
    }

    public class TestFacet
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
}
