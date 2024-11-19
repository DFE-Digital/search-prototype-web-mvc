namespace Dfe.Testing.Pages.DocumentQueryClient.Provider.AngleSharp;
internal class AngleSharpDocumentQueryClient : IDocumentQueryClient
{
    private readonly IHtmlDocument _htmlDocument;
    public AngleSharpDocumentQueryClient(IHtmlDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);
        _htmlDocument = document;
    }

    public void Run(QueryArgs args, Action<IDocumentPart> handler)
    {
        if (args.Scope == null)
        {
            handler(
                AsDocumentPart(
                    QueryForElementInScope(_htmlDocument, args.Query)));
            return;
        }

        var scope = QueryForElementInScope(scope: _htmlDocument, selector: args.Scope);
        handler(
            AsDocumentPart(
                QueryForElementInScope(scope, args.Query)));
    }

    public TResult Query<TResult>(QueryArgs queryArgs, Func<IDocumentPart, TResult> Mapper)
    {
        IElement element = queryArgs.Scope == null ?
            QueryForElementInScope(_htmlDocument, queryArgs.Query) :
                // find the scope and query within
                QueryForElementInScope(
                    scope: QueryForElementInScope(scope: _htmlDocument, selector: queryArgs.Scope),
                    selector: queryArgs.Query);

        return Mapper(
            AsDocumentPart(element));
    }


    public IEnumerable<TResult> QueryMany<TResult>(QueryArgs queryArgs, Func<IDocumentPart, TResult> Mapper)
    {
        IEnumerable<IElement> elements = queryArgs.Scope == null ?
            QueryForMultipleElementsFromScope(scope: _htmlDocument, selector: queryArgs.Query) :
                // find the scope and query within
                QueryForMultipleElementsFromScope(
                    scope: QueryForElementInScope(scope: _htmlDocument, selector: queryArgs.Scope),
                    selector: queryArgs.Query);

        return AsDocumentParts(elements)
            .Select(t => Mapper(t));
    }



    private static IElement QueryForElementInScope(IParentNode scope, IElementSelector selector)
    {
        var elements = scope.QuerySelectorAll(selector.ToSelector());
        if (elements == null || elements.Length == 0)
        {
            throw new ArgumentException($"No elements found in scope using selector {selector.ToSelector()}");
        }
        if (elements.Length > 1)
        {
            throw new ArgumentException($"Multiple elements found in scope using selector {selector.ToSelector()}");
        };
        return elements.Single();
    }

    private static IEnumerable<IElement> QueryForMultipleElementsFromScope(IParentNode scope, IElementSelector selector)
    {
        var elements = scope.QuerySelectorAll(selector.ToSelector());
        if (elements == null || elements.Length == 0)
        {
            throw new ArgumentException($"No elements found in scope using selector {selector.ToSelector()}");
        }
        return elements;
    }


    private static AngleSharpDocumentPart AsDocumentPart(IElement element) => new(element);
    private static IEnumerable<AngleSharpDocumentPart> AsDocumentParts(IEnumerable<IElement> elements) => elements.Select(AsDocumentPart);

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

        public void Click()
        {
            throw new NotImplementedException("Clicking is not available with an AngleSharp client");
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

        public IEnumerable<IDocumentPart> GetChildren() => _element.Children?.Select(t => (IDocumentPart)new AngleSharpDocumentPart(t)).ToList() ?? [];
        public IEnumerable<IDocumentPart> GetChildren(IElementSelector selector) 
            => _element.QuerySelectorAll(selector.ToSelector())
                .Select(AsDocumentPart)
                .ToList<IDocumentPart>();
    }
}