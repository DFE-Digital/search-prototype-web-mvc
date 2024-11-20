namespace Dfe.Testing.Pages.DocumentQueryClient;
public class QueryRequest
{
    public IElementSelector Query { get; }
    public IElementSelector? Scope { get; }
    public QueryRequest(
        IElementSelector query,
        IElementSelector? scope = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        Query = query;
        Scope = scope;
    }
}