using Dfe.Testing.Pages.DocumentQueryClient.Pages;
using Dfe.Testing.Pages.DocumentQueryClient.Pages.Components.AnchorLink;
using Dfe.Testing.Pages.DocumentQueryClient.Pages.Components.CheckboxInput;
using Dfe.Testing.Pages.DocumentQueryClient.Pages.Components.FieldSet;
using Dfe.Testing.Pages.DocumentQueryClient.Pages.Components.Form;

namespace Dfe.Testing.Pages.DocumentQueryClient;

internal static class DependencyInjection
{
    internal static IServiceCollection AddDocumentQueryClientPublicAPI<TProvider>(this IServiceCollection services) where TProvider : class, IDocumentQueryClientProvider
        => services
            .AddScoped<IDocumentQueryClientProvider, TProvider>()
            .AddScoped<IDocumentQueryClientAccessor, DocumentQueryClientAccessor>()
            // Pages
            .AddScoped<IPageFactory, PageFactory>()
            // Common Components
            .AddTransient<AnchorLinkComponentFactory>()
            .AddTransient<FormComponentFactory>()
            .AddTransient<FieldSetComponentFactory>()
            .AddTransient<CheckboxWithLabelComponentFactory>()
            // Helpers    
            .AddTransient<IHttpRequestBuilder, HttpRequestBuilder>();
}
