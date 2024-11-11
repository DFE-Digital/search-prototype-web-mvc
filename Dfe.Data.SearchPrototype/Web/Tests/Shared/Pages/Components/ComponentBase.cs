using DfE.Data.SearchPrototype.Web.Tests.Shared.DocumentQueryClient.Accessor;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient;
using DfE.Data.SearchPrototype.Web.Tests.Shared.Pages.Components.Link;

namespace DfE.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;

public abstract class ComponentBase
{
    private readonly IDocumentQueryClientAccessor _documentQueryClientAccessor;

    protected ComponentBase(IDocumentQueryClientAccessor documentQueryClientAccessor, LinkFactory linkFactory)
    {
        ArgumentNullException.ThrowIfNull(documentQueryClientAccessor);
        ArgumentNullException.ThrowIfNull(linkFactory);
        _documentQueryClientAccessor = documentQueryClientAccessor;
        LinkFactory = linkFactory;
    }

    protected IDocumentQueryClient DocumentQueryClient => _documentQueryClientAccessor.DocumentQueryClient;
    protected LinkFactory LinkFactory { get; }
}