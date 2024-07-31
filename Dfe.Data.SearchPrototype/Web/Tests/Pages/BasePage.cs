using Dfe.Data.SearchPrototype.Web.Tests.Acceptance.Drivers;

namespace Dfe.Data.SearchPrototype.Web.Tests.PageObjectModel;

public abstract class BasePage
{
    internal IWebDriverContext DriverContext { get; }

    public BasePage(IWebDriverContext driverContext)
    {
        DriverContext = driverContext ?? throw new ArgumentNullException(nameof(driverContext));
    }
}
