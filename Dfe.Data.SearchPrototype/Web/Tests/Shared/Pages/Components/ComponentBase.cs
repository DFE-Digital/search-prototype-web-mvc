using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient;
using DfE.Data.SearchPrototype.Web.Tests.Shared.DocumentQueryClient.Accessor;

namespace DfE.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;

public abstract class ComponentBase
{
    private readonly IDocumentQueryClientAccessor _documentQueryClientAccessor;

    protected ComponentBase(IDocumentQueryClientAccessor documentQueryClientAccessor)
    {
        ArgumentNullException.ThrowIfNull(documentQueryClientAccessor);
        _documentQueryClientAccessor = documentQueryClientAccessor;
    }
    internal abstract IQuerySelector Container { get; }
    protected IDocumentQueryClient DocumentQueryClient => _documentQueryClientAccessor.DocumentQueryClient;
}