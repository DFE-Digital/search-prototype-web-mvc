using Dfe.Data.SearchPrototype.Web.Tests.Shared;
using Dfe.Testing.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace Dfe.Data.SearchPrototype.Web.Tests.Web.Integration.HTTP.Tests.DependencyInjection;

internal sealed class HttpTestServices
{
    private static readonly HttpTestServices _instance = new();
    private readonly IServiceProvider _serviceProvider;
    static HttpTestServices()
    {
    }

    private HttpTestServices()
    {
        IServiceCollection services = new ServiceCollection()
            .AddPages()
            .AddAngleSharp<Program>();

        _serviceProvider = services.BuildServiceProvider();
    }

    public static HttpTestServices Instance
    {
        get
        {
            return _instance;
        }
    }

    internal IServiceScope CreateScope() => _serviceProvider.CreateScope();
}