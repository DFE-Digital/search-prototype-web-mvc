using OpenQA.Selenium;

namespace Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver.Internal;
// TODO should this be a command boundary, intake commands which can be stored to replay, observed on, logged

public interface IApplicationNavigator
{
    Task NavigateToAsync(Uri uri);
    Task BackAsync();
    Task ReloadAsync();
}

public interface IWebDriverAdaptor : IApplicationNavigator, IAsyncDisposable
{
    Task StartAsync();
    System.Net.Cookie? GetCookie(string cookieName);
    IEnumerable<System.Net.Cookie> GetCookies();
    Task TakeScreenshotAsync();
    // TODO need to adapt this to hide Selenium and return something mapped -- NOTE will need to be able to Find from the element
    // TODO extend to include FindOptions per request?
    IWebElement FindElement(IElementSelector selector);
    IReadOnlyCollection<IWebElement> FindElements(IElementSelector selector);
    //TODO something to mock a request?
}

// Wraps WebDriver operations with lazy init
internal sealed class LazyWebDriverAdaptor : IWebDriverAdaptor
{
    private readonly Lazy<IWebDriver> _driver;

    public LazyWebDriverAdaptor(Func<IWebDriver> getDriver)
    {
        ArgumentNullException.ThrowIfNull(getDriver, nameof(getDriver));
        _driver = new Lazy<IWebDriver>(getDriver);
    }
    private IWebDriver Driver => _driver.Value ?? throw new ArgumentNullException(nameof(_driver.Value));

    public Task StartAsync()
    {
        _ = Driver;
        return Task.CompletedTask;
    }

    public async Task NavigateToAsync(Uri uri) => await Driver.Navigate().GoToUrlAsync(uri);

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        using (Driver)
        {
            await Driver.Manage().Network.StopMonitoring();
            Driver.Quit();
        }
    }

    // TODO for these operations need to queue commands
    public Task BackAsync()
    {
        throw new NotImplementedException();
    }

    public Task ReloadAsync()
    {
        throw new NotImplementedException();
    }

    public System.Net.Cookie GetCookie(string cookieName)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<System.Net.Cookie> GetCookies()
    {
        throw new NotImplementedException();
    }

    public Task TakeScreenshotAsync()
    {
        throw new NotImplementedException();
    }

    public IWebElement FindElement(IElementSelector selector)
        => Driver.FindElement(
            WebDriverByLocatorHelpers.CreateLocator(selector));

    public IReadOnlyCollection<IWebElement> FindElements(IElementSelector selector) 
        => Driver.FindElements(
            WebDriverByLocatorHelpers.CreateLocator(selector));

}