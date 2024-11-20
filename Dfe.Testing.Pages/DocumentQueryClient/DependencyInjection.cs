using Dfe.Testing.Pages.DocumentQueryClient.Pages;
using Dfe.Testing.Pages.DocumentQueryClient.Pages.GDSComponents.AnchorLink;
using Dfe.Testing.Pages.DocumentQueryClient.Pages.GDSComponents.Buttons;
using Dfe.Testing.Pages.DocumentQueryClient.Pages.GDSComponents.CheckboxWithLabel;
using Dfe.Testing.Pages.DocumentQueryClient.Pages.GDSComponents.FieldSet;
using Dfe.Testing.Pages.DocumentQueryClient.Pages.GDSComponents.Form;

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
            .AddTransient<AnchorLinkFactory>()
            .AddTransient<FormFactory>()
            .AddTransient<FieldsetFactory>()
            .AddTransient<CheckboxWithLabelFactory>()
            .AddTransient<ButtonFactory>()
            // Helpers    
            .AddTransient<IHttpRequestBuilder, HttpRequestBuilder>();
}
