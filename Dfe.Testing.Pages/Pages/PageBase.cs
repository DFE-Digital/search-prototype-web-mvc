namespace Dfe.Testing.Pages.Pages;

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
