using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared;

public static class EstablishmentFacetTestDouble
{
    public static EstablishmentFacet Create()
    {
        var facetResultList = new List<FacetResult>() { new("primary",10), new("secondary",5), new("post 16", 2) };
        return new EstablishmentFacet("Education phase", facetResultList);
    }
}

