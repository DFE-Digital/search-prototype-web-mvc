namespace Dfe.Testing.Pages.DocumentQueryClient;
public class QueryArgs
{
    public IElementSelector Query { get; }
    public IElementSelector? Scope { get; }
    public QueryArgs(
        IElementSelector query,
        IElementSelector? scope = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        Query = query;
        Scope = scope;
    }
}