using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.DomQueryClient.Factory;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using DfE.Data.SearchPrototype.Web.Tests.Shared;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using static Dfe.Data.SearchPrototype.Web.Tests.Web.Integration.HTTP.Tests.HomePageTests;

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
    private readonly IServiceProvider _serviceProvider;
    internal TestServices()
    {
        IServiceCollection services = new ServiceCollection();
        services
            .AddScoped<CustomWebApplicationFactory>()
            .AddScoped<HttpRequestBuilder>()
            //.AddScoped<IDomQueryClientFactory, AngleSharpDomQueryClientFactory>() not being resolved through container as depends on HttpClient which is created from factory at runtime. WithWebHostBuilder() does not persist configuration
            
            // mocked SearchClient responses
            .AddScoped((provider) 
                => provider.GetService<CustomWebApplicationFactory>()!.Services.GetService<ISearchByKeywordClientProvider>()!);

        // TODO delaying the creation of the program so it can be overwritten in a test
        // AddPages() for DI?
        _serviceProvider = services.BuildServiceProvider();
    }

    internal IServiceScope CreateServiceResolverScope() => _serviceProvider.CreateScope();
}

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public CustomWebApplicationFactory()
    {

    }
}