using Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver;
using Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver.Internal.Provider;
using Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver.Internal.SessionOptions;
using Microsoft.Extensions.Options;

namespace Dfe.Testing.Pages;


public static class DependencyInjection
{
    // TODO Assumption that user will only register AngleSharp or Selenium with the queryClient. May eventually allow WebDriver too
    public static IServiceCollection AddAngleSharpQueryClient<TApplicationProgram>(this IServiceCollection services) where TApplicationProgram : class
        => services
                .AddExternalDocumentQueryClientAbstractionsCore()
                .AddScoped<IDocumentQueryClientProvider, AngleSharpDocumentQueryClientProvider>()
                .AddWebApplicationFactory<TApplicationProgram>();

    public static IServiceCollection AddWebDriver(this IServiceCollection services)
        => services
            .AddExternalDocumentQueryClientAbstractionsCore()
            .AddScoped<IDocumentQueryClientProvider, WebDriverDocumentQueryClientProvider>()
            .AddWebDriverInternals();

    private static IServiceCollection AddExternalDocumentQueryClientAbstractionsCore(this IServiceCollection services)
        => services
            .AddScoped<IDocumentQueryClientAccessor, DocumentQueryClientAccessor>()
            // Pages
            .AddScoped<IPageFactory, PageFactory>()
            // Common Components
            .AddTransient<LinkMapper>()
            // Helpers    
            .AddTransient<IHttpRequestBuilder, HttpRequestBuilder>();

    private static IServiceCollection AddWebApplicationFactory<TApplicationProgram>(this IServiceCollection services) where TApplicationProgram : class
        => services
            .AddScoped<WebApplicationFactory<TApplicationProgram>, TestServerFactory<TApplicationProgram>>()
            .AddScoped(scope => scope.GetRequiredService<WebApplicationFactory<TApplicationProgram>>().CreateClient())
            .AddScoped<IConfigureWebHostHandler, ConfigureWebHostHandler>();

    private static IServiceCollection AddWebDriverInternals(this IServiceCollection services)
        => services
            // Scoped to allow client to alter per test scope    
            .AddScoped<IOptions<WebDriverClientSessionOptions>>((serviceProvider) =>
            {
                WebDriverClientSessionOptions options = new();
                // Configure default options if needed
                return Options.Create(options);
            })
            .AddScoped<IWebDriverAdaptorProvider, CachedWebDriverAdaptorProvider>()
            .AddScoped<IApplicationNavigatorAccessor, ApplicationNavigatorAccessor>()
            .AddTransient<IWebDriverSessionOptionsBuilder, WebDriverSessionOptionsBuilder>();

}