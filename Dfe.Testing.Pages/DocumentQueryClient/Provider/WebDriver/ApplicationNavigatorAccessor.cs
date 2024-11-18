using Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver.Internal.Provider.Adaptor;

namespace Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver;

public interface IApplicationNavigatorAccessor
{
    public IApplicationNavigator Navigator { get; internal set; }
}

internal sealed class ApplicationNavigatorAccessor : IApplicationNavigatorAccessor
{
    private IApplicationNavigator? _navigator = null!;
    public IApplicationNavigator Navigator
    {
        get => _navigator ?? throw new ArgumentException("ApplicationNavigator has not been set", nameof(_navigator));
        set => _navigator = value;
    }
}
