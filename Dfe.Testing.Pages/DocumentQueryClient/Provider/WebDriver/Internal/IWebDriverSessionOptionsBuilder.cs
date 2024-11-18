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
    private int? _pageLoadTimeoutSeconds = default;
    private int? _requestTimeoutSeconds = default;
    private bool _enableNetworkInterception = false;
    public WebDriverSessionOptions Build()
    {
        WebDriverSessionOptions options = new();
        if (_pageLoadTimeoutSeconds.HasValue)
        {
            options.PageLoadTimeout = TimeSpan.FromSeconds(_pageLoadTimeoutSeconds.Value);
        }
        if (_requestTimeoutSeconds.HasValue)
        {
            options.RequestTimeout = TimeSpan.FromSeconds(_requestTimeoutSeconds.Value);
        }
        if (_enableNetworkInterception)
        {
            options.IsNetworkInterceptionEnabled = true;
        }
        return options;
    }

    public IWebDriverSessionOptionsBuilder WithBrowserType(string browserType)
    {
        MapBrowserType(browserType);
        return this;
    }

    public IWebDriverSessionOptionsBuilder WithNetworkInterception(bool enable)
    {
        _enableNetworkInterception = enable;
        return this;
    }

    public IWebDriverSessionOptionsBuilder WithPageLoadTimeout(int pageLoadTimeoutSeconds)
    {
        _pageLoadTimeoutSeconds = pageLoadTimeoutSeconds;
        return this;
    }

    public IWebDriverSessionOptionsBuilder WithRequestTimeout(int requestTimeoutSeconds)
    {
        _requestTimeoutSeconds = requestTimeoutSeconds;
        return this;
    }

    private static BrowserType MapBrowserType(string browserType)
    {
        string? normalised = browserType?.ToLowerInvariant() ?? string.Empty;
        return normalised switch
        {
            "chrome" => BrowserType.Chrome,
            "edge" => BrowserType.Edge,
            "firefox" => BrowserType.Firefox,
            _ => BrowserType.Chrome
        };
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