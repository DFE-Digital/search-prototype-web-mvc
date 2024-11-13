namespace Dfe.Testing.Pages.Pages.Components;
public abstract class ComponentBase
{
    private readonly IDocumentQueryClientAccessor _documentQueryClientAccessor;

    protected ComponentBase(IDocumentQueryClientAccessor documentQueryClientAccessor)
    {
        ArgumentNullException.ThrowIfNull(documentQueryClientAccessor);
        _documentQueryClientAccessor = documentQueryClientAccessor;
    }

    protected IDocumentQueryClient DocumentQueryClient => _documentQueryClientAccessor.DocumentQueryClient;
}
