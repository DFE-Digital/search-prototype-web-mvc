namespace Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver.Internal.Provider.Adaptor;
public interface IApplicationNavigator
{
    Task NavigateToAsync(Uri uri);
    Task BackAsync();
    Task ReloadAsync();
}