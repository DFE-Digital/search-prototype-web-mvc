namespace Dfe.Testing.Pages.DocumentQueryClient;

public sealed class QueryCommandBuilder<TResult>
{
    private IElementSelector? QueryInsideScope = null;
    private IElementSelector? Query = null;
    private Func<IDocumentPart, TResult>? Mapper = null;

    public QueryCommandBuilder<TResult> Create() => new();

    public QueryCommandBuilder<TResult> SetQueryInScope(IElementSelector selector)
    {
        QueryInsideScope = selector;
        return this;
    }

    public QueryCommandBuilder<TResult> SetQuery(IElementSelector selector)
    {
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));
        Query = selector;
        return this;
    }

    public QueryCommandBuilder<TResult> SetMapper(Func<IDocumentPart, TResult> mapper)
    {
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));
        Mapper = mapper;
        return this;
    }

    public QueryCommand<TResult> Build()
    {
        ArgumentNullException.ThrowIfNull(Query, nameof(Query));
        ArgumentNullException.ThrowIfNull(Mapper, nameof(Mapper));
        return new QueryCommand<TResult>(Query, Mapper, QueryInsideScope);
    }
}
