using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient.Factory;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using DfE.Data.SearchPrototype.Web.Tests.Shared;
using DfE.Data.SearchPrototype.Web.Tests.Shared.Pages;
using DfE.Data.SearchPrototype.Web.Tests.Shared.WebApplicationFactory;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using static Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.HomePage;

namespace Dfe.Data.SearchPrototype.Web.Tests.Web.Integration.HTTP.Tests;

public abstract class BaseHttpTest : IDisposable
{
    private static readonly TestServices services = new();
    private readonly IServiceScope _serviceScope;

    protected BaseHttpTest(ITestOutputHelper testOutputHelper)
    {
        _serviceScope = services.CreateServiceScopeResolver();
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
    private readonly IServiceProvider _serviceProvider;
    internal TestServices()
    {
        IServiceCollection services = new ServiceCollection()
            .AddScoped<IConfigureWebHostHandler, ConfigureWebHostHandler>()
            .AddScoped<WebApplicationFactory<Program>, TestServerFactory>()
            .AddScoped<IDocumentQueryClientProvider, AngleSharpDocumentQueryClientProvider>()
            .AddScoped<IDocumentQueryClientAccessor, DocumentQueryClientAccessor>()
            // AddPages() for DI or is this creator enough?
            .AddScoped<NavigationBarComponent>()
            .AddScoped<HomePage>()
            .AddScoped<IPageFactory, PageFactory>()
            .AddTransient<IHttpRequestBuilder, HttpRequestBuilder>();

        // TODO delaying the creation of the program so it can be overwritten in a test
        _serviceProvider = services.BuildServiceProvider();
    }

    internal IServiceScope CreateServiceScopeResolver() => _serviceProvider.CreateScope();
}

public interface IDocumentQueryClientAccessor
{
    IDocumentQueryClient DocumentQueryClient { get; set; }
}

public sealed class DocumentQueryClientAccessor : IDocumentQueryClientAccessor
{
    private IDocumentQueryClient? _documentQueryClient;
    public IDocumentQueryClient DocumentQueryClient
    {
        get => _documentQueryClient ?? throw new ArgumentNullException(nameof(_documentQueryClient));
        set => _documentQueryClient = value;
    }
}
