namespace Dfe.Testing.Pages.DocumentQueryClient.Pages.Components;
public abstract class ComponentFactoryBase<T> where T : IComponent
{
    private readonly IDocumentQueryClientAccessor _documentQueryClientAccessor;

    protected ComponentFactoryBase(IDocumentQueryClientAccessor documentQueryClientAccessor)
    {
        ArgumentNullException.ThrowIfNull(documentQueryClientAccessor);
        _documentQueryClientAccessor = documentQueryClientAccessor;
    }

    protected IDocumentQueryClient DocumentQueryClient => _documentQueryClientAccessor.DocumentQueryClient;
    public abstract List<T> GetMany(QueryRequest? request = null);
}
