using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using Microsoft.Extensions.DependencyInjection;
using Dfe.Testing.Pages;

namespace Dfe.Data.SearchPrototype.Web.Tests.Web.Integration.UI.Tests.DependencyInjection;

internal sealed class UIServices
{

    private static readonly UIServices _instance = new();
    private readonly IServiceProvider _serviceProvider;
    static UIServices()
    {
    }
    
    private UIServices()
    {
        IServiceCollection services = new ServiceCollection()
            .AddTransient<SearchComponent>()
            .AddTransient<NavigationBarComponent>()
            .AddTransient<HomePage>()
            .AddTransient<SearchResultsComponent>()
            .AddTransient<FilterComponent>()
            .AddWebDriver();
    
        _serviceProvider = services.BuildServiceProvider();
    }
    
    public static UIServices Instance
    {
        get
        {
            return _instance;
        }
    }
    
    internal IServiceScope CreateScope() => _serviceProvider.CreateScope();
}
    
internal abstract class BaseServices
{

}