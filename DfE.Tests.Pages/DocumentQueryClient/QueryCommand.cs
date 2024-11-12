namespace DfE.Tests.Pages.DocumentQueryClient;
public class QueryCommand<TResult>
{
    public IQuerySelector Query { get; }
    public Func<IDocumentPart, TResult> Processor { get; }
    public IQuerySelector? QueryScope { get; } = null;
    public QueryCommand(
        IQuerySelector query,
        Func<IDocumentPart, TResult> processor,
        IQuerySelector? queryScope = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(processor);
        Processor = processor;
        Query = query;
        QueryScope = queryScope;
    }
}