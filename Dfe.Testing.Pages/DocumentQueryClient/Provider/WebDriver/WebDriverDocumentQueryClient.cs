using OpenQA.Selenium;
using System.Collections.ObjectModel;

namespace Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver;

internal sealed class WebDriverDocumentQueryClient : IDocumentQueryClient
{
    private readonly IWebDriver _webDriver;

    public WebDriverDocumentQueryClient(IWebDriver webDriver)
    {
        ArgumentNullException.ThrowIfNull(webDriver);
        _webDriver = webDriver;
    }

    public TResult Query<TResult>(QueryCommand<TResult> queryCommand)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TResult> QueryMany<TResult>(QueryCommand<TResult> queryCommand)
    {
        throw new NotImplementedException();
    }

    private sealed class WebDriverDocumentPart : IDocumentPart
    {
        private const string childOfDocumentXpathPrefix = ".//";
        private readonly IWebElement _element;

        public WebDriverDocumentPart(IWebElement element)
        {
            _element = element;
        }

        public string Text
        {
            get => _element.Text ?? string.Empty;
            set => _element.SendKeys(value);
        }

        public string? GetAttribute(string attributeName)
        {
            ArgumentException.ThrowIfNullOrEmpty(attributeName);
            return _element.GetAttribute(attributeName);
        }

        public IDictionary<string, string> GetAttributes() => throw new NotImplementedException("TODO GetAttributes in WebDriver - parsing over the top or JS");

        public IDocumentPart? GetChild(IElementSelector selector) => FindDocumentPart(selector);

        public IEnumerable<IDocumentPart> GetChildren()
            => WrapElementsAsDocumentPart(
                FindMany(
                    CreateXPathSelector(childOfDocumentXpathPrefix)));

        private WebDriverDocumentPart FindDocumentPart(IElementSelector selector)
        {
            const string entireDocumentPrefix = "//";
            var selectWith = selector.ToSelector();

            By webDriverSelector =
                // infer if the locator mechanism is xpath
                (selectWith.StartsWith(entireDocumentPrefix) || selectWith.StartsWith(childOfDocumentXpathPrefix)) ?
                    CreateXPathSelector(selectWith) :
                    CreateCssSelector(selectWith);

            // TODO pass in an error message into the collection extensions?
            return WrapElementsAsDocumentPart(
                FindMany(webDriverSelector)
                    .ThrowIfNullOrEmpty()
                    .ThrowIfMultiple())
                    .Single();
        }

        private ReadOnlyCollection<IWebElement> FindMany(By by)  => _element.FindElements(by) ?? Array.Empty<IWebElement>().AsReadOnly();

        private static IEnumerable<WebDriverDocumentPart> WrapElementsAsDocumentPart(IEnumerable<IWebElement> elements) 
            => elements.Select(
                (element) => new WebDriverDocumentPart(element));

        private static By CreateXPathSelector(string selector) => By.XPath(selector);
        private static By CreateCssSelector(string selector) => By.CssSelector(selector);
    }
}
