namespace Dfe.Testing.Pages.DocumentQueryClient;

public sealed class QueryCommandBuilder<TResult>
{
    private IElementSelector? QueryInsideScope = null;
    private IElementSelector? Query = null;
    private Func<IDocumentPart, TResult>? Processor = null;

    public static QueryCommandBuilder<TResult> Create() => new();

    public QueryCommandBuilder<TResult> WithScope(IElementSelector selector)
    {
        QueryInsideScope = selector;
        return this;
    }

    public QueryCommandBuilder<TResult> WithQuery(IElementSelector selector)
    {
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));
        Query = selector;
        return this;
    }

    public QueryCommandBuilder<TResult> WithProcessor(Func<IDocumentPart, TResult> processor)
    {
        ArgumentNullException.ThrowIfNull(processor, nameof(processor));
        Processor = processor;
        return this;
    }

    public QueryCommand<TResult> Build()
    {
        ArgumentNullException.ThrowIfNull(Query, nameof(Query));
        ArgumentNullException.ThrowIfNull(Processor, nameof(Processor));
        return new QueryCommand<TResult>(Query, Processor, QueryInsideScope);
    }
}
