using Dfe.Data.SearchPrototype.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Data.SearchPrototype.Web.Controllers;


public class FacetController : Controller
{
    [HttpPost]
    public IActionResult GetFacets(FacetsViewModel facetsViewModel)
    {
        return View("Facet", facetsViewModel);
    }

    public IActionResult Index()
    {
        List<TestFacet> testFacets =
        [
            new(){ Name = "FACET_1", IsSelected= false},
            new(){ Name = "FACET_2", IsSelected= false},
            new(){ Name = "FACET_3", IsSelected= false},
            new(){ Name = "FACET_4", IsSelected= false}
        ];

        FacetsViewModel model = new()
        {
            TestFacets = testFacets
        };

        return View("Facet", model);
    }
}
