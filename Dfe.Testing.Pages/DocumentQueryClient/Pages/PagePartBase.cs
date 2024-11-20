namespace Dfe.Testing.Pages.DocumentQueryClient.Pages;
public abstract class PagePartBase
{
    private readonly IDocumentQueryClientAccessor _documentQueryClientAccessor;
    protected PagePartBase(IDocumentQueryClientAccessor documentQueryClientAccessor)
    {
        ArgumentNullException.ThrowIfNull(documentQueryClientAccessor, nameof(documentQueryClientAccessor));
        _documentQueryClientAccessor = documentQueryClientAccessor;
    }

    protected IDocumentQueryClient DocumentQueryClient
    {
        get => _documentQueryClientAccessor.DocumentQueryClient;
    }
}