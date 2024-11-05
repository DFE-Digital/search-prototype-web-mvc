using Dfe.Data.SearchPrototype.Web.Tests.Acceptance.Drivers;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;

public abstract class BasePage
{
    internal IWebDriverContext DriverContext { get; }
    protected IDomQueryClient DomQueryClient { get; }

    public BasePage(IDomQueryClient domQueryClient)
    {
        ArgumentNullException.ThrowIfNull(domQueryClient);
        DomQueryClient = domQueryClient;
    }
}
