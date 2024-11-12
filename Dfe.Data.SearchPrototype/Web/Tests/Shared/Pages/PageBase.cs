using Dfe.Data.SearchPrototype.Web.Tests.Acceptance.Drivers;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient;
using DfE.Data.SearchPrototype.Web.Tests.Shared.DocumentQueryClient.Accessor;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;

public abstract class PageBase
{
    private readonly IDocumentQueryClientAccessor _documentQueryClientAccessor;
    protected PageBase(IDocumentQueryClientAccessor documentQueryClientAccessor)
    {
        ArgumentNullException.ThrowIfNull(documentQueryClientAccessor, nameof(documentQueryClientAccessor));
        _documentQueryClientAccessor = documentQueryClientAccessor;
    }

    protected IDocumentQueryClient DocumentQueryClient
    {
        get => _documentQueryClientAccessor.DocumentQueryClient;
    }
}
