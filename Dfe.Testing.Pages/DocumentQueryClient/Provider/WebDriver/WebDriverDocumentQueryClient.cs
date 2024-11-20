using Dfe.Testing.Pages.WebDriver.Internal;
using Dfe.Testing.Pages.WebDriver.Internal.Provider.Adaptor;
using OpenQA.Selenium;
using System.Collections.ObjectModel;

namespace Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver;

internal sealed class WebDriverDocumentQueryClient : IDocumentQueryClient
{
    private readonly IWebDriverAdaptor _webDriverAdaptor;

    public WebDriverDocumentQueryClient(IWebDriverAdaptor webDriverAdaptor)
    {
        ArgumentNullException.ThrowIfNull(webDriverAdaptor);
        _webDriverAdaptor = webDriverAdaptor;
    }

    public void Run(QueryRequest args, Action<IDocumentPart> handler)
    {
        if (args.Scope == null)
        {
            handler(
                WebDriverDocumentPart.Create(
                    Find(args.Query)));
            return;
        }

        handler(
            WebDriverDocumentPart.Create(
                Find(args.Scope)
                    .FindElement(
                        WebDriverByLocatorHelpers.CreateLocator(args.Query))));
    }

    public TResult Query<TResult>(QueryRequest queryArgs, Func<IDocumentPart, TResult> mapper)
    {
        ArgumentNullException.ThrowIfNull(queryArgs);
        ArgumentNullException.ThrowIfNull(mapper);
        IDocumentPart? documentPartToMap =
            WebDriverDocumentPart.Create(
                queryArgs.Scope == null ?
                    _webDriverAdaptor.FindElement(queryArgs.Query!) :
                    _webDriverAdaptor.FindElement(queryArgs.Scope!)
                        .FindElement(
                            WebDriverByLocatorHelpers.CreateLocator(queryArgs.Query)));

        return mapper(documentPartToMap);
    }

    public IEnumerable<TResult> QueryMany<TResult>(QueryRequest queryArgs, Func<IDocumentPart, TResult> mapper)
    {
        ArgumentNullException.ThrowIfNull(queryArgs);
        ArgumentNullException.ThrowIfNull(mapper);
        IEnumerable<IWebElement>? elements =
                queryArgs.Scope == null ?
                    _webDriverAdaptor.FindElements(queryArgs.Query!) :
                    _webDriverAdaptor.FindElement(queryArgs.Scope!)
                        .FindElements(
                            WebDriverByLocatorHelpers.CreateLocator(queryArgs.Query));

        return elements
            .Select(WebDriverDocumentPart.Create)
            .Select(mapper)
            // TODO expression is evaluated immediately on IEnumerable<T> because otherwise stale references to elements that have changed...
            // Could improve design by capturing scope inside of DocumentPart so it's retryable if reference chanes?
            .ToList();
    }

    private IWebElement Find(IElementSelector selector) => _webDriverAdaptor.FindElement(selector);


    private sealed class WebDriverDocumentPart : IDocumentPart
    {
        private readonly IWebElement _wrappedElement;

        public WebDriverDocumentPart(IWebElement element)
        {
            ArgumentNullException.ThrowIfNull(element);
            _wrappedElement = element;
        }

        public static WebDriverDocumentPart Create(IWebElement element) => new(element);

        public string Text
        {
            get => _wrappedElement.Text ?? string.Empty;
            set => _wrappedElement.SendKeys(value);
        }

        public string TagName => _wrappedElement.TagName;

        public bool HasAttribute(string attributeName) => GetAttribute(attributeName) != null;
        public string? GetAttribute(string attributeName)
        {
            ArgumentException.ThrowIfNullOrEmpty(attributeName);
            return _wrappedElement.GetAttribute(attributeName);
        }

        public IDictionary<string, string> GetAttributes() => throw new NotImplementedException("TODO GetAttributes in WebDriver - parsing over the top or JS");

        public IDocumentPart? GetChild(IElementSelector selector) => FindDocumentPart(selector);

        public IEnumerable<IDocumentPart> GetChildren()
            => AsDocumentPart(
                FindMany(
                    WebDriverByLocatorHelpers.AsXPath(new ChildXPathSelector())))
                .ToList<IDocumentPart>();

        public IEnumerable<IDocumentPart> GetChildren(IElementSelector selector)
            => FindMany(
                    WebDriverByLocatorHelpers.CreateLocator(selector))
                .Select(Create)
                .ToList<IDocumentPart>();

        private WebDriverDocumentPart FindDocumentPart(IElementSelector selector)
            // TODO pass in an error message into the collection extensions?
            => AsDocumentPart(
                FindMany(
                    WebDriverByLocatorHelpers.CreateLocator(selector))
                .ThrowIfNullOrEmpty()
                .ThrowIfMultiple())
                .Single();

        private ReadOnlyCollection<IWebElement> FindMany(By by)
            => _wrappedElement.FindElements(by) ?? Array.Empty<IWebElement>().AsReadOnly();

        private static IEnumerable<WebDriverDocumentPart> AsDocumentPart(IEnumerable<IWebElement> elements)
            => elements?.Select(
                (element) => new WebDriverDocumentPart(element)) ?? [];

        public void Click() => _wrappedElement.Click();
    }
}