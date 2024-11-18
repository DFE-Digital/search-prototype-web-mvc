using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using Dfe.Testing.Pages;
using Dfe.Testing.Pages.DocumentQueryClient.Provider;
using Dfe.Testing.Pages.Pages;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.EndToEndTests;

public sealed class ExampleUITest
{
    public ExampleUITest()
    {

    }

    [Fact]
    public async Task TestConsumptionOf_DriverInfrastructure_Alone()
    {
        ServiceCollection services = new();
        services.AddPagesCore()
            .AddWebDriver()
            .AddScoped<HomePage>();

        var provider = services.BuildServiceProvider();
        var pageFactory = provider.GetRequiredService<IPageFactory>();
        await pageFactory.CreatePageAsync<HomePage>(
            new HttpRequestMessage()
            {
                RequestUri = new("https://searchprototype.azurewebsites.net/")
            });

        var something = 2;
    }
}
