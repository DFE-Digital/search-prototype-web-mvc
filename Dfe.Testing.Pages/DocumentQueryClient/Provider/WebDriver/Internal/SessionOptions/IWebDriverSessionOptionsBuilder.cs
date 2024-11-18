namespace Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver.Internal.SessionOptions;
internal interface IWebDriverSessionOptionsBuilder
{
    IWebDriverSessionOptionsBuilder WithBrowserType(string browserType);
    IWebDriverSessionOptionsBuilder WithPageLoadTimeout(int pageLoadTimeoutSeconds);
    IWebDriverSessionOptionsBuilder WithRequestTimeout(int requestTimeoutSeconds);
    IWebDriverSessionOptionsBuilder WithNetworkInterception(bool enable);
    WebDriverSessionOptions Build();
}