using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient.Factory;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using DfE.Data.SearchPrototype.Web.Tests.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Dfe.Data.SearchPrototype.Web.Tests.Web.Integration.HTTP.Tests;

public abstract class BaseHttpTest : IDisposable
{
    private static readonly TestServices services = new();
    private readonly IServiceScope _serviceScope;

    protected BaseHttpTest(ITestOutputHelper testOutputHelper)
    {
        _serviceScope = services.CreateServiceResolverScope();
        TestOutputHelper = testOutputHelper;
    }

    protected ITestOutputHelper TestOutputHelper { get; }

    protected T GetTestService<T>()
        => _serviceScope.ServiceProvider.GetService<T>()
            ?? throw new ArgumentNullException($"Unable to resolve type {typeof(T)}");

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _serviceScope.Dispose();
    }
}

internal sealed class TestServices
{
    IServiceProvider _serviceProvider;
    internal TestServices()
    {
        IServiceCollection services = new ServiceCollection()
            .AddScoped<IConfigureWebHostHandler, ConfigureWebHostHandler>()
            .AddScoped<TestServerFactory>()
            /*.AddSingleton<CustomWebApplicationFactory>()
            .AddScoped<WebApplicationFactoryHttpClient>()*/
            .AddScoped<HttpRequestBuilder>()
            .AddScoped<IDocumentQueryClientProvider, AngleSharpDocumentClientProvider>()
            // AddPages() for DI or is this creator enough?
            .AddScoped<IPageFactory, PageFactory>();

        // TODO delaying the creation of the program so it can be overwritten in a test
        _serviceProvider = services.BuildServiceProvider();
    }

    internal IServiceScope CreateServiceResolverScope() => _serviceProvider.CreateScope();
}

public class TestServerFactory : WebApplicationFactory<Program>
{
    private readonly IConfigureWebHostHandler _configureWebHostHandler;

    public TestServerFactory(IConfigureWebHostHandler configureWebHostHandler)
    {
        _configureWebHostHandler = configureWebHostHandler;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        var configure = _configureWebHostHandler.Create();
        configure(builder);
    }
}

public interface IConfigureWebHostHandler
{
    Action<IWebHostBuilder> Create();
    void SetConfigure(Action<IWebHostBuilder> configure);
}

public sealed class ConfigureWebHostHandler : IConfigureWebHostHandler
{
    private Action<IWebHostBuilder>? _configure;
    public void SetConfigure(Action<IWebHostBuilder> configure) => _configure = configure;

    public Action<IWebHostBuilder> Create() => _configure ?? new Action<IWebHostBuilder>(builder => { }); // NOOP if not set
}

public sealed class PageFactory : IPageFactory
{
    private readonly IDocumentQueryClientProvider _documentClientFactory;

    public PageFactory(IDocumentQueryClientProvider documentClientFactory)
    {
        _documentClientFactory = documentClientFactory;
    }
    public async Task<TPage> CreatePageAsync<TPage>(HttpRequestMessage httpRequestMessage) where TPage : BasePage, new()
    {
        TPage page = new()
        {
            DocumentClient = await _documentClientFactory.CreateDocumentClientAsync(httpRequestMessage)
        };
        return page;
    }
}

public interface IPageFactory
{
    public Task<TPage> CreatePageAsync<TPage>(HttpRequestMessage httpRequest) where TPage : BasePage, new();
}