namespace Dfe.Testing.Pages.DocumentQueryClient.Provider.AngleSharp;
internal class AngleSharpDocumentQueryClient : IDocumentQueryClient
{
    private readonly IHtmlDocument _htmlDocument;
    public AngleSharpDocumentQueryClient(IHtmlDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);
        _htmlDocument = document;
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
        if (command.QueryScope == null)
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

    private static IElement QueryFromScope(IParentNode parent, IElementSelector queryLocator)
         => parent.QuerySelectorAll(queryLocator.ToSelector())
                .ThrowIfNullOrEmpty()
                .ThrowIfMultiple()
                .Single();

    private static IEnumerable<IElement> QueryForMultipleFromScope(IParentNode parent, IElementSelector queryLocator)
         => parent.QuerySelectorAll(
                    queryLocator.ToSelector())
                .ThrowIfNullOrEmpty();


    private sealed class AngleSharpDocumentPart : IDocumentPart
    {
        private readonly IElement _element;

        public AngleSharpDocumentPart(IElement element)
        {
            ArgumentNullException.ThrowIfNull(element);
            _element = element;
        }

        public string Text
        {
            get => _element.TextContent ?? string.Empty;
            set => _element.TextContent = value;
        }

        public string? GetAttribute(string attributeName)
        {
            ArgumentNullException.ThrowIfNull(attributeName);
            return _element.GetAttribute(attributeName);
        }

        public IDictionary<string, string> GetAttributes()
            => _element.Attributes?.ToDictionary(
                    keySelector: (attr) => attr.Name,
                    elementSelector: (attr) => attr.Value) ?? [];

        public IDocumentPart? GetChild(IElementSelector selector)
        {
            ArgumentNullException.ThrowIfNull(selector);
            IElement? child = _element.QuerySelector(selector.ToSelector());
            return null == child ? null : new AngleSharpDocumentPart(child);
        }

        public IEnumerable<IDocumentPart> GetChildren() => _element.Children?.Select(t => (IDocumentPart)new AngleSharpDocumentPart(t)) ?? [];
    }
}