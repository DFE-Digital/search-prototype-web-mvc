using OpenQA.Selenium;

namespace Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver.Internal;
// TODO should this be a command boundary, intake commands which can be stored to replay, observed on, logged

public interface IApplicationNavigator
{
    Task NavigateToAsync(Uri uri);
    Task BackAsync();
    Task ReloadAsync();
}

public interface IWebDriverAdaptor : IApplicationNavigator, IDisposable, IAsyncDisposable
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
internal class LazyWebDriverAdaptor : IWebDriverAdaptor
{
    private readonly Lazy<IWebDriver> _getDriver;

    public LazyWebDriverAdaptor(Func<IWebDriver> getDriver)
    {
        ArgumentNullException.ThrowIfNull(getDriver, nameof(getDriver));
        _getDriver = new Lazy<IWebDriver>(getDriver);
    }
    private IWebDriver Driver => _getDriver?.Value ?? throw new ArgumentNullException(nameof(_getDriver.Value));

    public Task StartAsync()
    {
        _ = Driver;
        return Task.CompletedTask;
    }

    public async Task NavigateToAsync(Uri uri) => await Driver.Navigate().GoToUrlAsync(uri);

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);

        Dispose(disposing: false);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            using (Driver)
            {
                Driver?.Quit();
            }
        }
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        using (Driver)
        {
            await Driver.Manage().Network.StopMonitoring().ConfigureAwait(false);
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