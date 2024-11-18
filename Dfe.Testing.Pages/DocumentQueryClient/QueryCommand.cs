namespace Dfe.Testing.Pages.DocumentQueryClient;
public class QueryCommand<TResult>
{
    public IElementSelector Query { get; }
    public Func<IDocumentPart, TResult> MapToResult { get; }
    public IElementSelector? QueryInScope { get; }
    public QueryCommand(
        IElementSelector query,
        Func<IDocumentPart, TResult> Mapper,
        IElementSelector? queryScope = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(Mapper);
        MapToResult = Mapper;
        Query = query;
        QueryInScope = queryScope;
    }
}
