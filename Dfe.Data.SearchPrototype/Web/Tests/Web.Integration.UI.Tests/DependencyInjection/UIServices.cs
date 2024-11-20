﻿using Microsoft.Extensions.DependencyInjection;
using Dfe.Testing.Pages;
using Microsoft.Extensions.Configuration;
using Dfe.Data.SearchPrototype.Web.Tests.Web.Integration.UI.Tests.Options;
using Microsoft.Extensions.Options;
using Dfe.Testing.Pages.WebDriver;
using Dfe.Data.SearchPrototype.Web.Tests.Shared;

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
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("test.ui.settings.json", optional: false)
                .Build();

        IServiceCollection services = new ServiceCollection()
            .AddWebDriver()
            .Configure<WebDriverClientSessionOptions>(config.GetRequiredSection("WebDriverClientSessionOptions"))
                .AddScoped(sp => sp.GetRequiredService<IOptions<WebDriverClientSessionOptions>>().Value)
            .AddPages()
            // application options
            .Configure<UIApplicationOptions>(config.GetRequiredSection("ApplicationOptions"))
                .AddSingleton(sp => sp.GetRequiredService<IOptions<UIApplicationOptions>>().Value); ;
    
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
