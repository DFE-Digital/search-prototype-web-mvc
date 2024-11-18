using Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver.Internal.Provider.Adaptor;

namespace Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver.Internal.Provider;
internal interface IWebDriverAdaptorProvider
{
    Task<IWebDriverAdaptor> CreateAsync();
}