using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;
using Dfe.Testing.Pages;
using Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver;
using Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver.Internal;
using Dfe.Testing.Pages.Pages;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.EndToEndTests;

public sealed class ExampleUITest
{
    private readonly ServiceProvider _serviceProvider;

    public ExampleUITest()
    {
        ServiceCollection services = new();
        services
            .AddWebDriver()
            .AddTransient<SearchComponent>()
            .AddTransient<NavigationBarComponent>()
            .AddTransient<HomePage>()
            .AddTransient<SearchResultsComponent>();

        _serviceProvider = services.BuildServiceProvider();
    }

    [Fact]
    public async Task TestConsumptionOf_FullQUeryClient_AsyncScope()
    {
        using var scope = _serviceProvider.CreateAsyncScope();
        var pageFactory = scope.ServiceProvider.GetRequiredService<IPageFactory>();

        HttpRequestMessage request = new()
        {
            RequestUri = new("https://searchprototype.azurewebsites.net/")
        };

        // TODO option->navigateToUri = true to dictate to the provider if it should create a document or reuse an existing document
        // internal logic across providers to have a CachedDocumentFactory?
        var homePage = await pageFactory.CreatePageAsync<HomePage>(request);
        homePage.Search.GetSubheading().Should().Be("Search establishments");
    }

    [Fact]
    public async Task SHOULD_FAIL_TestConsumptionOf_FullQUeryClient_AsyncScope()
    {
        using var scope = _serviceProvider.CreateAsyncScope();
        var pageFactory = scope.ServiceProvider.GetRequiredService<IPageFactory>();

        HttpRequestMessage request = new()
        {
            RequestUri = new("https://searchprototype.azurewebsites.net/")
        };

        // TODO option->navigateToUri = true to dictate to the provider if it should create a document or reuse an existing document
        // internal logic across providers to have a CachedDocumentFactory?
        var homePage = await pageFactory.CreatePageAsync<HomePage>(request);
        homePage.Search.GetSubheading().Should().Be("SHOULD FAIL HERE");
    }

    [Fact]
    public async Task TestConsumptionOf_DriverInfrastructure_Alone()
    {
        using var scope = _serviceProvider.CreateScope();
        var pageFactory = scope.ServiceProvider.GetRequiredService<IPageFactory>();

        HttpRequestMessage request = new()
        {
            RequestUri = new("https://searchprototype.azurewebsites.net/")
        };

        // TODO option->navigateToUri = true to dictate to the provider if it should create a document or reuse an existing document
        // internal logic across providers to have a CachedDocumentFactory?
        var homePage = await pageFactory.CreatePageAsync<HomePage>(request);
        homePage.Search.GetSubheading().Should().Be("Search establishments");
    }

    [Fact]
    public async Task SHOULD_FAIL_TestConsumptionOf_FullQUeryClient_SyncScope()
    {
        using var scope = _serviceProvider.CreateScope();
        var pageFactory = scope.ServiceProvider.GetRequiredService<IPageFactory>();

        HttpRequestMessage request = new()
        {
            RequestUri = new("https://searchprototype.azurewebsites.net/")
        };

        // TODO option->navigateToUri = true to dictate to the provider if it should create a document or reuse an existing document
        // internal logic across providers to have a CachedDocumentFactory?
        var homePage = await pageFactory.CreatePageAsync<HomePage>(request);
        homePage.Search.GetSubheading().Should().Be("SHOULD_FAIL");
    }


    [Fact]
    public async Task TestConsumption_Of_Navigator()
    {
        using var scope = _serviceProvider.CreateScope();
        var pageFactory = scope.ServiceProvider.GetRequiredService<IPageFactory>();
        HttpRequestMessage request = new()
        {
            RequestUri = new("https://searchprototype.azurewebsites.net/")
        };
        var homePage = await pageFactory.CreatePageAsync<HomePage>(request);

        var navigatorProxy = scope.ServiceProvider.GetRequiredService<IApplicationNavigatorAccessor>();
        await navigatorProxy.Navigator.NavigateToAsync(new Uri("https://searchprototype.azurewebsites.net/?searchKeyWord=Col"));
        var something = "x";
    }
}
