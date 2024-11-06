using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient.Factory;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
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
    protected HttpClient ServerHttpClient
    {
        get => _serviceScope.ServiceProvider.GetService<HttpClient>() ?? throw new ArgumentNullException(nameof(ServerHttpClient));
    }

    protected T ResolveService<T>()
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
    private static readonly WebApplicationFactory<Program> factory = new();
    private readonly IServiceProvider _serviceProvider;
    internal TestServices()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddSingleton<HttpClient>(_ => factory.CreateClient())
            .AddScoped<IDomQueryClientFactory, AngleSharpDomQueryClientFactory>();
        // AddPages() for DI?
        _serviceProvider = services.BuildServiceProvider();
    }

    internal IServiceScope CreateServiceResolverScope() => _serviceProvider.CreateScope();
}