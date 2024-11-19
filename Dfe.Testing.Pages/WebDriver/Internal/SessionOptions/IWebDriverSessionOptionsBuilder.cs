namespace Dfe.Testing.Pages.WebDriver.Internal.SessionOptions;
internal interface IWebDriverSessionOptionsBuilder
{
    IWebDriverSessionOptionsBuilder WithBrowserType(string browserType);
    IWebDriverSessionOptionsBuilder WithPageLoadTimeout(int pageLoadTimeoutSeconds);
    IWebDriverSessionOptionsBuilder WithRequestTimeout(int requestTimeoutSeconds);
    IWebDriverSessionOptionsBuilder WithNetworkInterception(bool enable);
    WebDriverSessionOptions Build();
}