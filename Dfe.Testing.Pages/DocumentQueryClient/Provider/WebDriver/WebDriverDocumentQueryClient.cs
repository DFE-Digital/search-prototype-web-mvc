using Dfe.Testing.Pages.DocumentQueryClient.Selector.Extensions;
using Microsoft.Extensions.Options;
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
                    AsXPath(new ChildXPathSelector())));

        private WebDriverDocumentPart FindDocumentPart(IElementSelector selector)
        {
            By webDriverSelector = selector.IsSelectorXPathConvention() ?
                    AsXPath(selector) :
                    AsCssSelector(selector);

            // TODO pass in an error message into the collection extensions?
            return WrapElementsAsDocumentPart(
                FindMany(webDriverSelector)
                    .ThrowIfNullOrEmpty()
                    .ThrowIfMultiple())
                    .Single();
        }

        private ReadOnlyCollection<IWebElement> FindMany(By by) => _element.FindElements(by) ?? Array.Empty<IWebElement>().AsReadOnly();

        private static IEnumerable<WebDriverDocumentPart> WrapElementsAsDocumentPart(IEnumerable<IWebElement> elements)
            => elements.Select(
                (element) => new WebDriverDocumentPart(element));

        private static By AsXPath(IElementSelector selector) => By.XPath(selector.ToSelector());
        private static By AsCssSelector(IElementSelector selector) => By.CssSelector(selector.ToSelector());
    }
}

internal interface IWebDriverProvider
{
    // TODO consider a wrapper around operations to not leak WebDriver out
    Task<IWebDriver> CreateWebDriver(WebDriverSessionOptions options);
}

public sealed class WebDriverProvider : IWebDriverProvider
{
    private readonly IOptions<WebDriverSessionOptions> _defaultWebDriverSessionOptions;

    public WebDriverProvider(IOptions<WebDriverSessionOptions> defaultWebDriverSessionOptions)
    {
        _defaultWebDriverSessionOptions = defaultWebDriverSessionOptions;
    }
    public Task<IWebDriver> CreateWebDriver(WebDriverSessionOptions options)
    {
        throw new NotImplementedException();
    }
}
// TODO consder an external bindable object for JSON configuration which we validate and map to our internal WebDriverSessionOptions

public class WebDriverSessionOptions
{
    public BrowserType BrowserType { get; set; }
    public TimeSpan PageLoadTimeout { get; set; }
    public TimeSpan RequestTimeout { get; set; }
    public bool IsNetworkInterceptionEnabled { get; set; }
    // TODO should the options be a list or dict<list> mapping? { chrome: { ... }, { edge: { ... }, {default: {...}
    public IDictionary<BrowserType, IEnumerable<string>> BrowserOptions { get; set; } = new Dictionary<BrowserType, IEnumerable<string>>();
}

public enum BrowserType
{
    Chrome,
    Firefox,
    Edge
}

internal static class BrowserTypeExtensions
{
    internal static string ToBrowserName(this BrowserType browserType)
        => browserType switch
        {
            BrowserType.Chrome => "chrome",
            BrowserType.Firefox => "firefox",
            BrowserType.Edge => "edge",
            _ => throw new NotImplementedException($"unsupported browser type {browserType}")
        };
}