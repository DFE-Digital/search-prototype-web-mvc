using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using Dfe.Testing.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Dfe.Data.SearchPrototype.Web.Tests.Web.Integration.HTTP.Tests.DependencyInjection;

internal sealed class TestServices
{
    private static readonly TestServices _instance = new();
    private readonly IServiceProvider _serviceProvider;
    static TestServices()
    {
    }

    private TestServices()
    {
        IServiceCollection services = new ServiceCollection()
            .AddTransient<SearchComponent>()
            .AddTransient<NavigationBarComponent>()
            .AddTransient<HomePage>()
            .AddTransient<SearchResultsComponent>()
            .AddAngleSharpQueryClient<Program>();

        _serviceProvider = services.BuildServiceProvider();
    }

    public static TestServices Instance
    {
        get
        {
            return _instance;
        }
    }

    internal IServiceScope CreateScope() => _serviceProvider.CreateScope();
}