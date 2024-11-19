using Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver.Internal.Provider.Adaptor;
using Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver.Internal.Provider.WebDriverFactory;
using Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver.Internal.SessionOptions;
using Microsoft.Extensions.Options;

namespace Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver.Internal.Provider;
internal sealed class CachedWebDriverAdaptorProvider : IWebDriverAdaptorProvider, IDisposable, IAsyncDisposable
{
    private static readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly WebDriverClientSessionOptions _webDriverClientSessionOptions;
    private readonly IWebDriverSessionOptionsBuilder _webDriverSessionOptionsBuilder;
    private readonly IApplicationNavigatorAccessor _applicationNavigatorAccessor;
    private IWebDriverAdaptor? _cachedWebDriverInstance = null;
    public CachedWebDriverAdaptorProvider(
        WebDriverClientSessionOptions webDriverClientSessionOptions,
        IWebDriverSessionOptionsBuilder webDriverSessionOptionsBuilder,
        IApplicationNavigatorAccessor applicationNavigatorAccessor)
    {
        ArgumentNullException.ThrowIfNull(webDriverClientSessionOptions);
        ArgumentNullException.ThrowIfNull(webDriverSessionOptionsBuilder);
        ArgumentNullException.ThrowIfNull(applicationNavigatorAccessor);
        _webDriverClientSessionOptions = webDriverClientSessionOptions;
        _webDriverSessionOptionsBuilder = webDriverSessionOptionsBuilder;
        _applicationNavigatorAccessor = applicationNavigatorAccessor;
    }

    public async Task<IWebDriverAdaptor> CreateAsync()
    {
        if (_cachedWebDriverInstance == null)
        {
            try
            {
                // TODO switch on options.BrowserType to choose which concrete WebDriverFactory
                var factory = new ChromeDriverFactory();
                await _semaphore.WaitAsync();
                // TODO browser options Dictionary
                // TODO browser version
                WebDriverSessionOptions sessionOptions = _webDriverSessionOptionsBuilder
                    .WithBrowserType(_webDriverClientSessionOptions.BrowserName)
                    .WithNetworkInterception(_webDriverClientSessionOptions.EnableNetworkInterception)
                    .WithPageLoadTimeout(_webDriverClientSessionOptions.PageLoadTimeout)
                    .WithRequestTimeout(_webDriverClientSessionOptions.RequestTimeout)
                    .Build();

                _cachedWebDriverInstance = new LazyWebDriverAdaptor(
                    getDriver: await factory.CreateDriver(sessionOptions),
                    sessionOptions);

                _applicationNavigatorAccessor.Navigator = _cachedWebDriverInstance;
            }
            finally
            {
                _semaphore.Release();
            }
        }
        return _cachedWebDriverInstance;
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
            _cachedWebDriverInstance?.Dispose();
            _cachedWebDriverInstance = null;

            if (_cachedWebDriverInstance is IDisposable disposable)
            {
                disposable.Dispose();
                _cachedWebDriverInstance = null;
            }
        }
    }

    async ValueTask DisposeAsyncCore()
    {
        if (_cachedWebDriverInstance is not null)
        {
            await _cachedWebDriverInstance.DisposeAsync().ConfigureAwait(false);
        }

        if (_cachedWebDriverInstance is IAsyncDisposable disposable)
        {
            await disposable.DisposeAsync().ConfigureAwait(false);
        }
        else
        {
            _cachedWebDriverInstance?.Dispose();
        }
        _cachedWebDriverInstance = null;
    }
}
