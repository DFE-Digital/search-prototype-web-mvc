using Dfe.Testing.Pages.Pages.Factory;

namespace Dfe.Testing.Pages;

public static class DependencyInjection
{
    public static IServiceCollection AddPages<TApplicationProgram>(this IServiceCollection services) where TApplicationProgram : class
        =>  // Web application factory and httpclient
            services.AddScoped<WebApplicationFactory<TApplicationProgram>, TestServerFactory<TApplicationProgram>>()
                .AddScoped(scope => scope.GetRequiredService<WebApplicationFactory<TApplicationProgram>>().CreateClient())
                .AddScoped<IConfigureWebHostHandler, ConfigureWebHostHandler>()
                // DocumentQueryClient
                .AddScoped<IDocumentQueryClientProvider, AngleSharpDocumentQueryClientProvider>()
                .AddScoped<IDocumentQueryClientAccessor, DocumentQueryClientAccessor>()
                // Pages
                .AddScoped<IPageFactory, PageFactory>()
                // Components
                .AddTransient<LinkQueryCommand>()
                .AddTransient<IHttpRequestBuilder, HttpRequestBuilder>();
}

