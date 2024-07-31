using Dfe.Data.SearchPrototype.Web.Controllers;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.TestDoubles;

public static class LoggerTestDouble
{
    public static Mock<ILogger<HomeController>> MockLogger()
    {
        Mock<ILogger<HomeController>> mockLogger = new();
        return mockLogger;
    }
}
