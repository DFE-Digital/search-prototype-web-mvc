using Microsoft.Extensions.Options;

namespace Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver.Internal;
internal interface IWebDriverAdaptorProvider
{
    Task<IWebDriverAdaptor> CreateAsync();
}

internal sealed class CachedWebDriverAdaptorProvider : IWebDriverAdaptorProvider, IDisposable, IAsyncDisposable
{
    private static readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly WebDriverClientSessionOptions _webDriverClientSessionOptions;
    private readonly IWebDriverSessionOptionsBuilder _webDriverSessionOptionsBuilder;
    private readonly IApplicationNavigatorAccessor _applicationNavigatorAccessor;
    private IWebDriverAdaptor? _instance = null;
    public CachedWebDriverAdaptorProvider(
        IOptions<WebDriverClientSessionOptions> webDriverClientSessionOptions,
        IWebDriverSessionOptionsBuilder webDriverSessionOptionsBuilder,
        IApplicationNavigatorAccessor applicationNavigatorAccessor)
    {
        ArgumentNullException.ThrowIfNull(webDriverClientSessionOptions);
        ArgumentNullException.ThrowIfNull(webDriverSessionOptionsBuilder);
        ArgumentNullException.ThrowIfNull(applicationNavigatorAccessor);
        _webDriverClientSessionOptions = webDriverClientSessionOptions.Value;
        _webDriverSessionOptionsBuilder = webDriverSessionOptionsBuilder;
        _applicationNavigatorAccessor = applicationNavigatorAccessor;
    }

    //TODO WebDriverSessionOptionsBuilder mapping in from an external POCO
    public async Task<IWebDriverAdaptor> CreateAsync()
    {
        //WebDriverSessionOptions options = _webDriverSessionOptionsBuilder.WithNetworkInterception(options.)
        // switch on options.BrowserType to choose which concrete WebDriverFactory, wrap the result in a Lazy() and pass it into the WebDriverAdaptor.

        //TODO handle monitoring here as part of the Lazy delegate - await driver.Manage().Network.StartMonitoring();
        // TODO use the WebDriverSessionOptionsBuilder to pass in the sessionOptions into the factory
        if (_instance == null)
        {
            try
            {
                var factory = new ChromeDriverFactory();
                await _semaphore.WaitAsync();
                // TODO browser options Dictionary and browser version
                WebDriverSessionOptions sessionOptions = _webDriverSessionOptionsBuilder
                    .WithBrowserType(_webDriverClientSessionOptions.BrowserName)
                    .WithNetworkInterception(_webDriverClientSessionOptions.EnableNetworkInterception)
                    .WithPageLoadTimeout(_webDriverClientSessionOptions.PageLoadTimeout)
                    .WithRequestTimeout(_webDriverClientSessionOptions.RequestTimeout)
                    .Build();

                _instance = new LazyWebDriverAdaptor(
                    getDriver: await factory.CreateDriver(sessionOptions));

                _applicationNavigatorAccessor.Navigator = _instance;
            }
            finally
            {
                _semaphore.Release();
            }
        }
        return _instance;
    }


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

    void Dispose(bool disposing)
    {
        if (disposing)
        {
            _instance?.Dispose();
            _instance = null;

            if (_instance is IDisposable disposable)
            {
                disposable.Dispose();
                _instance = null;
            }
        }
    }

    async ValueTask DisposeAsyncCore()
    {
        if (_instance is not null)
        {
            await _instance.DisposeAsync().ConfigureAwait(false);
        }

        if (_instance is IAsyncDisposable disposable)
        {
            await disposable.DisposeAsync().ConfigureAwait(false);
        }
        else
        {
            _instance?.Dispose();
        }

        _instance = null;
        _instance = null;
    }
}
