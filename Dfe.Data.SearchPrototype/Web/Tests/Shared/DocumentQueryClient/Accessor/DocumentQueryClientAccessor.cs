using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient;

namespace DfE.Data.SearchPrototype.Web.Tests.Shared.DocumentQueryClient.Accessor;

public sealed class DocumentQueryClientAccessor : IDocumentQueryClientAccessor
{
    private IDocumentQueryClient? _documentQueryClient;
    public IDocumentQueryClient DocumentQueryClient
    {
        get => _documentQueryClient ?? throw new ArgumentNullException(nameof(_documentQueryClient));
        set => _documentQueryClient = value;
    }
}
