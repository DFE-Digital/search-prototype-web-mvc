namespace Dfe.Testing.Pages.DocumentQueryClient;
public sealed class QueryRequest
{
    public IElementSelector? Query { get; set; }
    public IElementSelector? Scope { get; set; }
}