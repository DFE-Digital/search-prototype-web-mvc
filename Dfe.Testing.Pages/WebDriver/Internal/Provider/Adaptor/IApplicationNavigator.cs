namespace Dfe.Testing.Pages.WebDriver.Internal.Provider.Adaptor;
public interface IApplicationNavigator
{
    Task NavigateToAsync(Uri uri);
    Task BackAsync();
    Task ReloadAsync();
}