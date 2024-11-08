using Dfe.Data.SearchPrototype.Web.Tests.Acceptance.Drivers;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;

public abstract class BasePage
{
    private IDocumentClient? _documentClient;
    protected internal IDocumentClient DocumentClient
    {
        get => _documentClient ?? throw new ArgumentNullException(nameof(_documentClient), "documentClient has not been set ");
        set => _documentClient = value ?? throw new ArgumentNullException(nameof(_documentClient));
    }
}
