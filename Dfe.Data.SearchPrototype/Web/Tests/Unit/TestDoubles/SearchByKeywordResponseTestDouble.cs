using Dfe.Data.SearchPrototype.SearchForEstablishments;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Web.Tests.Shared;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.TestDoubles;

public static class SearchByKeywordResponseTestDouble
{
    public static SearchByKeywordResponse Create()
    {
        List<Establishment> establishmentResults = new();
        for (int i = 0; i < new Bogus.Faker().Random.Int(1, 10); i++)
        {
            establishmentResults.Add(EstablishmentTestDouble.Create());
        }
        return new SearchByKeywordResponse(establishmentResults);
       
    }

    public static SearchByKeywordResponse CreateWithNoResults()
    {
        return new SearchByKeywordResponse(null!);
    }
}
