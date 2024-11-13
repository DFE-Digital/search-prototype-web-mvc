namespace Dfe.Testing.Pages;


public static class DependencyInjection
{
    public static IServiceCollection AddPages<TApplicationProgram>(this IServiceCollection services) where TApplicationProgram : class
        =>  // WebApplicationFactory and HttpClient
            services.AddScoped<WebApplicationFactory<TApplicationProgram>, TestServerFactory<TApplicationProgram>>()
                .AddScoped(scope => scope.GetRequiredService<WebApplicationFactory<TApplicationProgram>>().CreateClient())
                .AddScoped<IConfigureWebHostHandler, ConfigureWebHostHandler>()
                // DocumentQueryClient TODO defaulting to AngleSharp provider until another provider
                .AddScoped<IDocumentQueryClientProvider, AngleSharpDocumentQueryClientProvider>()
                .AddScoped<IDocumentQueryClientAccessor, DocumentQueryClientAccessor>()
                // Pages
                .AddScoped<IPageFactory, PageFactory>()
                // Components
                .AddTransient<LinkQueryCommand>()
                // Helpers
                .AddTransient<IHttpRequestBuilder, HttpRequestBuilder>();
}
