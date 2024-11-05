using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient.Factory;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.Web.Integration.HTTP.Tests;

public abstract class BaseHttpTest : IAsyncLifetime
{
    private HttpClient? _httpClient;
    private static readonly WebApplicationFactory<Program> factory = new();
    private static readonly TestServices services = new(factory);
    private readonly IServiceScope _serviceScope;

    protected BaseHttpTest(ITestOutputHelper testOutputHelper)
    {
        _serviceScope = services.CreateServiceResolverScope();
        TestOutputHelper = testOutputHelper;
    }

    protected HttpClient ServerHttpClient
    {
        get => _serviceScope.ServiceProvider.GetService<HttpClient>() ?? throw new ArgumentNullException(nameof(ServerHttpClient));
    }
    protected ITestOutputHelper TestOutputHelper { get; }

    protected T ResolveTestService<T>() 
        => _serviceScope.ServiceProvider.GetService<T>() 
            ?? throw new ArgumentNullException($"Unable to resolve type {typeof(T)}");

    public Task InitializeAsync()
    {
        // TODO
        return Task.CompletedTask;
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        _serviceScope.Dispose();
        return Task.CompletedTask;
    }

}

public sealed class TestServices
{
    private readonly IServiceProvider _serviceProvider;
    public TestServices(WebApplicationFactory<Program> factory)
    {
        IServiceCollection services = new ServiceCollection();
        services.AddSingleton<HttpClient>(_ => factory.CreateClient())
            .AddScoped<IDomQueryClientFactory, AngleSharpDomQueryClientFactory>() // Requires a IHtmlDocument and makes a request for it which isn't obvious to the caller.
            .AddScoped<HomePage>();
        // AddPages() for DI
        _serviceProvider = services.BuildServiceProvider();
    }

    public IServiceScope CreateServiceResolverScope() => _serviceProvider.CreateScope();
}