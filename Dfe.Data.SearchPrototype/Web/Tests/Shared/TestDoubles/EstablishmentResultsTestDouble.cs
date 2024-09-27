using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;

public static class EstablishmentResultsTestDouble
{
    public static EstablishmentResults Create()
    {
        var establishments = new List<Establishment>();

        for (int i = 0; i < new Bogus.Faker().Random.Int(1, 10); i++)
        {
            establishments.Add(
                EstablishmentTestDouble.Create());
        }
        return new EstablishmentResults(establishments);
    }

    public static EstablishmentResults CreateWithNoResults()
    {
        return new EstablishmentResults();
    }
}
