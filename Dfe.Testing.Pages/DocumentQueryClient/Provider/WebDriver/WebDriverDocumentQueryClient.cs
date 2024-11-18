using Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver.Internal;
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

    public TResult Query<TResult>(QueryCommand<TResult> queryCommand)
    {
        IDocumentPart? documentPartToMap =
            WebDriverDocumentPart.Create(
                queryCommand.QueryInScope == null ?
                    _webDriverAdaptor.FindElement(queryCommand.Query) :
                    _webDriverAdaptor.FindElement(queryCommand.QueryInScope)
                        .FindElement(
                            WebDriverByLocatorHelpers.CreateLocator(queryCommand.Query)));

        return queryCommand.MapToResult(documentPartToMap);
    }

    public IEnumerable<TResult> QueryMany<TResult>(QueryCommand<TResult> queryCommand)
    {
        IEnumerable<IWebElement> queryResults = 
            queryCommand.QueryInScope == null ?
                _webDriverAdaptor.FindElements(queryCommand.Query) :
                _webDriverAdaptor.FindElement(queryCommand.QueryInScope)
                    .FindElements(
                        WebDriverByLocatorHelpers.CreateLocator(queryCommand.Query));

        return queryResults
            .Select(WebDriverDocumentPart.Create)
            .Select(t => queryCommand.MapToResult(t));
    }

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
                    WebDriverByLocatorHelpers.AsXPath(new ChildXPathSelector())));

        private WebDriverDocumentPart FindDocumentPart(IElementSelector selector)
            // TODO pass in an error message into the collection extensions?
            => AsDocumentPart(
                FindMany(
                    WebDriverByLocatorHelpers.CreateLocator(selector))
                .ThrowIfNullOrEmpty()
                .ThrowIfMultiple())
                .Single();

        private ReadOnlyCollection<IWebElement> FindMany(By by) => _wrappedElement.FindElements(by) ?? Array.Empty<IWebElement>().AsReadOnly();

        private static IEnumerable<WebDriverDocumentPart> AsDocumentPart(IEnumerable<IWebElement> elements)
            => elements?.Select(
                (element) => new WebDriverDocumentPart(element)) ?? [];
    }
}