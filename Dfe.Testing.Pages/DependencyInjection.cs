using Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver;
using Dfe.Testing.Pages.DocumentQueryClient.Provider.WebDriver.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Dfe.Testing.Pages;


public static class DependencyInjection
{
    public static IServiceCollection AddPagesCore(this IServiceCollection services)
        => services.AddScoped<IDocumentQueryClientAccessor, DocumentQueryClientAccessor>()
                // Pages
                .AddScoped<IPageFactory, PageFactory>()
                // Components
                .AddTransient<LinkQueryCommand>();

    public static IServiceCollection AddHttpServices<TApplicationProgram>(this IServiceCollection services) where TApplicationProgram : class
        =>  // WebApplicationFactory and HttpClient
            services.AddScoped<WebApplicationFactory<TApplicationProgram>, TestServerFactory<TApplicationProgram>>()
                .AddScoped(scope => scope.GetRequiredService<WebApplicationFactory<TApplicationProgram>>().CreateClient())
                .AddScoped<IConfigureWebHostHandler, ConfigureWebHostHandler>();

    public static IServiceCollection AddAngleSharp(this IServiceCollection services)
        => services.AddScoped<IDocumentQueryClientProvider, AngleSharpDocumentQueryClientProvider>()
                // Helpers
                .AddTransient<IHttpRequestBuilder, HttpRequestBuilder>();

    public static IServiceCollection AddWebDriver(this IServiceCollection services)
    {
        // Scoped to allow client to alter per test scope
        services
            .AddScoped<IOptions<WebDriverClientSessionOptions>>((serviceProvider) =>
            {
                WebDriverClientSessionOptions options = new();
                // Configure default options if needed
                return Options.Create(options);
            })
            .AddScoped<IDocumentQueryClientProvider, WebDriverDocumentQueryClientProvider>()
            .AddScoped<IWebDriverAdaptorProvider, CachedWebDriverAdaptorProvider>()
            .AddScoped<IApplicationNavigatorAccessor, ApplicationNavigatorAccessor>()
            .AddTransient<IWebDriverSessionOptionsBuilder, WebDriverSessionOptionsBuilder>();
        return services;
    }
}