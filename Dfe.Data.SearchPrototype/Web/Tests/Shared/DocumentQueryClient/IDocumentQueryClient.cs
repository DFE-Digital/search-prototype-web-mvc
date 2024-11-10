namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient;

public interface IDocumentQueryClient
{
    // TODO could delete all of these primtives and wrap extensions around Query<TResult>
    string? GetText(string cssSelector);
    string? GetLink(string cssSelector);
    string? GetAttribute(string cssSelector, string attribute);
    IEnumerable<string?>? GetTexts(string cssSelector);
    bool ElementExists(string cssSelector);
    int GetCount(string criteria);

    // END TODO
    TResult Query<TResult>(QueryCommand<TResult> queryCommand);
    IEnumerable<TResult> QueryMany<TResult>(QueryCommand<TResult> queryCommand);
}

public class QueryCommand<TResult>
{
    public IQueryLocator Query { get; }
    public Func<IDocumentPart, TResult> Processor { get; }
    public IQueryLocator? QueryScope { get; } = null;
    public QueryCommand(
        IQueryLocator query, 
        Func<IDocumentPart, TResult> processor, 
        IQueryLocator? scope = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(processor);
        Processor = processor;
        Query = query;
        QueryScope = scope;
    }
}


public interface IQueryLocator
{
    string ToSelector();
}

public sealed class CssSelector : IQueryLocator
{
    private readonly string _locator;

    public CssSelector(string locator)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(locator, nameof(locator));
        _locator = locator;
    }

    public string ToSelector() => _locator;
    public override string ToString() => ToSelector();
}

public interface IDocumentPart
{
    string Text { get; set; }
    string GetAttribute(string attributeName);
    //IDictionary<string, string> GetAttributes();
    IEnumerable<IDocumentPart> GetChildren();
    IDocumentPart? GetChild(IQueryLocator selector);
}
