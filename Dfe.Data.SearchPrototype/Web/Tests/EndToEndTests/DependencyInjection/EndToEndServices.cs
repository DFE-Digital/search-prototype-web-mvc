using Microsoft.Extensions.DependencyInjection;
using Dfe.Testing.Pages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Dfe.Data.SearchPrototype.Web.Tests.Shared;
using Dfe.Data.SearchPrototype.Web.Tests.EndToEndTests.Options;
using Dfe.Testing.Pages.WebDriver;

namespace Dfe.Data.SearchPrototype.Web.Tests.EndToEndTests.DependencyInjection;
internal sealed class EndToEndServices
{
    private static readonly EndToEndServices _instance = new();
    private readonly IServiceProvider _serviceProvider;
    static EndToEndServices()
    {
    }

    private EndToEndServices()
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("testsettings.json", optional: false)
                .Build();

        IServiceCollection services = new ServiceCollection()
            .AddWebDriver()
            .Configure<WebDriverClientSessionOptions>(config.GetRequiredSection("WebDriverClientSessionOptions"))
                .AddScoped(sp => sp.GetRequiredService<IOptions<WebDriverClientSessionOptions>>().Value)
            .AddPages()
            // application options
            .Configure<ApplicationOptions>(config.GetRequiredSection("ApplicationOptions"))
                .AddSingleton(sp => sp.GetRequiredService<IOptions<ApplicationOptions>>().Value);


        _serviceProvider = services.BuildServiceProvider();
    }

    public static EndToEndServices Instance
    {
        get
        {
            return _instance;
        }
    }

    internal IServiceScope CreateScope() => _serviceProvider.CreateScope();
}