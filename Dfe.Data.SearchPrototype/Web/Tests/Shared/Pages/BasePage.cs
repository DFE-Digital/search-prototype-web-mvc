using Dfe.Data.SearchPrototype.Web.Tests.Acceptance.Drivers;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;

public abstract class BasePage
{
    internal IWebDriverContext DriverContext { get; }
    protected IDocumentClient DomQueryClient { get; }

    public BasePage(IDocumentClient domQueryClient)
    {
        ArgumentNullException.ThrowIfNull(domQueryClient);
        DomQueryClient = domQueryClient;
    }
}
