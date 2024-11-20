using Dfe.Testing.Pages.WebDriver.Internal.Provider.Adaptor;

namespace Dfe.Testing.Pages.WebDriver;

public interface IApplicationNavigatorAccessor
{
    public IApplicationNavigator Navigator { get; internal set; }
}
