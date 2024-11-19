using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using Microsoft.Extensions.DependencyInjection;
using Dfe.Testing.Pages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Dfe.Data.SearchPrototype.Web.Tests.Shared;

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
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("testsettings.json", optional: false)
                .Build();

        IServiceCollection services = new ServiceCollection()
            .AddWebDriver()
            .AddPages()
            // application options
            .Configure<ApplicationOptions>(config.GetRequiredSection("ApplicationOptions"))
                .AddSingleton(sp => sp.GetRequiredService<IOptions<ApplicationOptions>>().Value);


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