using Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver.Internal;

namespace Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver;

internal sealed class WebDriverDocumentQueryClientProvider : IDocumentQueryClientProvider
{
    private readonly IWebDriverAdaptorProvider _webDriverAdaptorProvider;

    public WebDriverDocumentQueryClientProvider(IWebDriverAdaptorProvider webDriverAdaptorProvider)
    {
        _webDriverAdaptorProvider = webDriverAdaptorProvider;
    }
    public async Task<IDocumentQueryClient> CreateDocumentClientAsync(HttpRequestMessage httpRequestMessage)
    {
        ArgumentNullException.ThrowIfNull(httpRequestMessage, nameof(httpRequestMessage));
        ArgumentNullException.ThrowIfNull(httpRequestMessage.RequestUri, nameof(httpRequestMessage.RequestUri));
        var webDriver = await _webDriverAdaptorProvider.CreateAsync();
        ArgumentNullException.ThrowIfNull(webDriver, nameof(webDriver));
        // TODO options will determine if we navigate
        await webDriver.NavigateToAsync(httpRequestMessage.RequestUri);
        return new WebDriverDocumentQueryClient(webDriver);
    }
}
