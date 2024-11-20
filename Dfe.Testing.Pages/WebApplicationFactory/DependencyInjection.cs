namespace Dfe.Testing.Pages.WebApplicationFactory;

internal static class DependencyInjection
{
    internal static IServiceCollection AddWebApplicationFactory<TApplicationProgram>(this IServiceCollection services) where TApplicationProgram : class
        => services
            .AddScoped<WebApplicationFactory<TApplicationProgram>, TestServerFactory<TApplicationProgram>>()
            .AddScoped<HttpClient>(scope => scope.GetRequiredService<WebApplicationFactory<TApplicationProgram>>().CreateClient())
            .AddScoped<IConfigureWebHostHandler, ConfigureWebHostHandler>();
}
