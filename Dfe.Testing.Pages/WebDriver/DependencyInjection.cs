using Dfe.Testing.Pages.WebDriver.Internal.Provider;
using Dfe.Testing.Pages.WebDriver.Internal.SessionOptions;
using Dfe.Testing.Pages.WebDriver.Internal;

namespace Dfe.Testing.Pages.WebDriver;

internal static class DependencyInjection
{
    internal static IServiceCollection AddWebDriverServices(this IServiceCollection services)
        => services
            .AddPublicAPI()
            .AddInternals();

    private static IServiceCollection AddPublicAPI(this IServiceCollection services)
        =>
        services
            // Scoped to allow client to alter per test scope
            .AddScoped<WebDriverClientSessionOptions>()
            .AddScoped<IApplicationNavigatorAccessor, ApplicationNavigatorAccessor>();

    private static IServiceCollection AddInternals(this IServiceCollection services)
        => services
            .AddScoped<IWebDriverAdaptorProvider, CachedWebDriverAdaptorProvider>()
            .AddTransient<IWebDriverSessionOptionsBuilder, WebDriverSessionOptionsBuilder>();
}
