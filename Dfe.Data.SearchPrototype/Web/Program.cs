using Dfe.Data.Common.Infrastructure.CognitiveSearch;
using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.Infrastructure;
using Dfe.Data.SearchPrototype.Infrastructure.Options;
using Dfe.Data.SearchPrototype.SearchForEstablishments;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.Web.Mappers;
using Dfe.Data.SearchPrototype.Web.Models.Factories;
using Dfe.Data.SearchPrototype.Web.Models.ViewModels;
using Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared;
using Dfe.Data.SearchPrototype.Web.Options;
using GovUk.Frontend.AspNetCore;
using Microsoft.Extensions.Options;
using Models = Dfe.Data.SearchPrototype.Web.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddViewLocalization(options => options.ResourcesPath = "Resources" );
builder.Services.AddGovUkFrontend();

// Start of IOC container registrations
//
//
builder.Services.AddDefaultSearchFilterServices(builder.Configuration);
builder.Services.AddDefaultCognitiveSearchServices(builder.Configuration);
builder.Services.AddCognitiveSearchAdaptorServices();
builder.Services.AddSearchForEstablishmentServices(builder.Configuration);
builder.Services.AddScoped<ISearchResultsFactory, SearchResultsFactory>();
builder.Services.AddSingleton<IMapper<EstablishmentResults?, List<Models.ViewModels.Establishment>?>, EstablishmentResultsToEstablishmentsViewModelMapper>();
builder.Services.AddSingleton<IMapper<FacetsAndSelectedFacets, List<Facet>?>, FacetsAndSelectedFacetsToFacetsViewModelMapper>();
builder.Services.AddSingleton<IMapper<Dictionary<string, List<string>>, IList<FilterRequest>?>, SelectedFacetsToFilterRequestsMapper>();
builder.Services.AddSingleton<IMapper<(int, int), Pagination>, PaginationResultsToPaginationViewModelMapper>();
builder.Services.AddSingleton<IPager, ScrollablePager>();
builder.Services.AddOptions<PaginationOptions>()
    .Configure<IOptions<AzureSearchOptions>>(
        (paginationOptions, azureSearchOptions) => {
            ArgumentNullException.ThrowIfNull(azureSearchOptions.Value);
            paginationOptions.RecordsPerPage = azureSearchOptions.Value.Size;
        });
//
//
// End of IOC container registrations

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

namespace Dfe.Data.SearchPrototype.Web
{
    public partial class Program { }
}