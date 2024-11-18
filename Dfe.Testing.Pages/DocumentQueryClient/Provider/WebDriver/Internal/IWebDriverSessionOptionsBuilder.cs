namespace Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver.Internal;
internal interface IWebDriverSessionOptionsBuilder
{
    IWebDriverSessionOptionsBuilder WithBrowserType(string browserType);
    IWebDriverSessionOptionsBuilder WithPageLoadTimeout(int pageLoadTimeoutSeconds);
    IWebDriverSessionOptionsBuilder WithRequestTimeout(int requestTimeoutSeconds);
    IWebDriverSessionOptionsBuilder WithNetworkInterception(bool enable);
    WebDriverSessionOptions Build();
}

internal sealed class WebDriverSessionOptionsBuilder : IWebDriverSessionOptionsBuilder
{
    public WebDriverSessionOptions Build()
    {
        throw new NotImplementedException();
    }

    public IWebDriverSessionOptionsBuilder WithBrowserType(string browserType)
    {
        throw new NotImplementedException();
    }

    public IWebDriverSessionOptionsBuilder WithNetworkInterception(bool enable)
    {
        throw new NotImplementedException();
    }

    public IWebDriverSessionOptionsBuilder WithPageLoadTimeout(int pageLoadTimeoutSeconds)
    {
        throw new NotImplementedException();
    }

    public IWebDriverSessionOptionsBuilder WithRequestTimeout(int requestTimeoutSeconds)
    {
        throw new NotImplementedException();
    }
}

public class WebDriverSessionOptions
{
    public BrowserType BrowserType { get; set; }
    public TimeSpan PageLoadTimeout { get; set; }
    public TimeSpan RequestTimeout { get; set; }
    public bool IsNetworkInterceptionEnabled { get; set; }
    // TODO should the options be a list or dict<list> mapping? { chrome: { ... }, { edge: { ... }, {default: {...}
    public IDictionary<BrowserType, IEnumerable<string>> BrowserOptions { get; set; } = new Dictionary<BrowserType, IEnumerable<string>>();
}