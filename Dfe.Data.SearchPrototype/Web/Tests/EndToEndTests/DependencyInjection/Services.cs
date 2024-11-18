using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using Microsoft.Extensions.DependencyInjection;
using Dfe.Testing.Pages;

namespace Dfe.Data.SearchPrototype.Web.Tests.EndToEndTests.DependencyInjection;
internal sealed class Services
{
    private static readonly Services _instance = new();
    private readonly IServiceProvider _serviceProvider;
    static Services()
    {
    }

    private Services()
    {
        IServiceCollection services = new ServiceCollection()
            .AddTransient<SearchComponent>()
            .AddTransient<NavigationBarComponent>()
            .AddTransient<HomePage>()
            .AddTransient<SearchResultsComponent>()
            .AddWebDriver();

        _serviceProvider = services.BuildServiceProvider();
    }

    public static Services Instance
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