using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.Pages.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared;

internal static class DependencyInjection
{
    public static IServiceCollection AddPages(this IServiceCollection services)
        => services.AddTransient<SearchComponent>()
            .AddTransient<NavigationBarComponent>()
            .AddTransient<HomePage>()
            .AddTransient<SearchResultsComponent>()
            .AddTransient<FilterComponent>();
}
