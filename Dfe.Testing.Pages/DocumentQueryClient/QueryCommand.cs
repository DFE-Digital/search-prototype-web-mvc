namespace Dfe.Testing.Pages.DocumentQueryClient;
public class QueryCommand<TResult>
{
    public IElementSelector Query { get; }
    public Func<IDocumentPart, TResult> Processor { get; }
    public IElementSelector? QueryScope { get; } = null;
    public QueryCommand(
        IElementSelector query,
        Func<IDocumentPart, TResult> processor,
        IElementSelector? queryScope = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(processor);
        Processor = processor;
        Query = query;
        QueryScope = queryScope;
    }
}
