using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient.Extensions;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient;

internal class AngleSharpDocumentQueryClient : IDocumentQueryClient
{
    private readonly IHtmlDocument _htmlDocument;
    public AngleSharpDocumentQueryClient(IHtmlDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);
        _htmlDocument = document;
    }

    public virtual string GetText(string cssSelector)
        => Find(cssSelector).TextContent.Trim();

    public virtual IEnumerable<string> GetTexts(string cssSelector)
        => FindAll(cssSelector)
            .Select(t => t.TextContent.Trim());

    public virtual string? GetAttribute(string cssSelector, string attribute)
    {
        ArgumentException.ThrowIfNullOrEmpty(attribute);
        return Find(cssSelector).GetAttribute(attribute);
    }
    public virtual string? GetLink(string cssSelector) => GetAttribute(cssSelector, "href");
    public bool ElementExists(string cssSelector) => TryFindOrDefault(cssSelector) != null;
    public int GetCount(string criteria) => FindAll(criteria).Count();
    private IElement Find(string cssSelector)
        => FindAll(cssSelector)
            .ThrowIfMultiple()
            .Single();

    private IEnumerable<IElement> FindAll(string cssSelector) => TryFindOrDefault(cssSelector)! ?? [];

    private IEnumerable<IElement>? TryFindOrDefault(string cssSelector)
    {
        ArgumentException.ThrowIfNullOrEmpty(cssSelector);
        return _htmlDocument.QuerySelectorAll(cssSelector);
    }

    public TResult Query<TResult>(QueryCommand<TResult> command)
    {
        if (command.QueryScope == null)
        {
            return command.Processor(
                new AngleSharpDocumentPart(
                    element: QueryFromScope(
                        _htmlDocument, command.Query)));
        }

        IElement scoped = _htmlDocument.QuerySelector(
            command.QueryScope.ToSelector()) ?? throw new ArgumentException($"could not find document part {command.QueryScope.ToSelector()}");

        return command.Processor(
            new AngleSharpDocumentPart(
                QueryFromScope(
                    scoped, command.Query)));

    }

    public IEnumerable<TResult> QueryMany<TResult>(QueryCommand<TResult> command)
    {
        if(command.QueryScope == null)
        {
            return QueryForMultipleFromScope(_htmlDocument, command.Query)
                    .Select(
                        (element) => command.Processor(new AngleSharpDocumentPart(element)));
        }

        var scope = _htmlDocument.QuerySelector(command.QueryScope.ToSelector()) 
            ?? throw new ArgumentNullException($"scope not found {command.QueryScope.ToSelector()}", nameof(command.QueryScope));
        
        var store = QueryForMultipleFromScope(scope, command.Query)
                .Select(
                    (element) => command.Processor(new AngleSharpDocumentPart(element)));
        return store;
    }

    private static IElement QueryFromScope(IParentNode parent, IQuerySelector queryLocator)
         => parent.QuerySelectorAll(queryLocator.ToSelector())
                .ThrowIfNullOrEmpty()
                .ThrowIfMultiple()
                .Single();

    private static IEnumerable<IElement> QueryForMultipleFromScope(IParentNode parent, IQuerySelector queryLocator)
         => parent.QuerySelectorAll(queryLocator.ToSelector())
            .ThrowIfNullOrEmpty();

}

public sealed class AngleSharpDocumentPart : IDocumentPart
{
    private readonly IElement _element;

    public AngleSharpDocumentPart(IElement element)
    {
        _element = element;
    }

    public string Text
    {
        get => _element.TextContent;
        set => _element.TextContent = value;
    }

    public string GetAttribute(string attributeName)
        => _element.GetAttribute(attributeName ?? throw new ArgumentNullException(nameof(attributeName))) ?? string.Empty;

    public IDocumentPart? GetChild(IQuerySelector selector)
    {
        IElement? child = _element.QuerySelector(selector.ToSelector());
        return null == child ? null : new AngleSharpDocumentPart(child);
    }

    public IEnumerable<IDocumentPart> GetChildren()
        => _element.Children?.Select(t => (IDocumentPart)new AngleSharpDocumentPart(t)) ?? [];
}