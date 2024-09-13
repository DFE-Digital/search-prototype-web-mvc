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
        FacetsViewModel model = new() { TheTestFacets = [
            new TestFacet()
            {
                Name = "THE_MAIN_TEST_FACET_1",
                FacetValues = [
                    new(){ Value = "FACET_1", IsSelected = false },
                    new(){ Value = "FACET_2", IsSelected = false },
                    new(){ Value = "FACET_3", IsSelected = false },
                    new(){ Value = "FACET_4", IsSelected = false }
                ]
            },
            new TestFacet()
            {
                Name = "THE_MAIN_TEST_FACET_2",
                FacetValues = [
                    new(){ Value = "FACET_5", IsSelected = false },
                    new(){ Value = "FACET_6", IsSelected = false },
                    new(){ Value = "FACET_7", IsSelected = false },
                    new(){ Value = "FACET_8", IsSelected = false }
                ]
            }
         ]};

        return View("Facet", model);
    }
}
