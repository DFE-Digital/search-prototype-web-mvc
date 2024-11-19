namespace Dfe.Testing.Pages.DocumentQueryClient;
public class ElementQueryArguments
{
    public IElementSelector Query { get; }
    public IElementSelector? Scope { get; }
    public ElementQueryArguments(
        IElementSelector query,
        IElementSelector? scope = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        Query = query;
        Scope = scope;
    }
}