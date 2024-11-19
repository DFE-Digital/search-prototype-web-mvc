using Dfe.Testing.Pages.WebDriver.Internal.Provider.Adaptor;

namespace Dfe.Testing.Pages.WebDriver.Internal.Provider;
internal interface IWebDriverAdaptorProvider
{
    Task<IWebDriverAdaptor> CreateAsync();
}