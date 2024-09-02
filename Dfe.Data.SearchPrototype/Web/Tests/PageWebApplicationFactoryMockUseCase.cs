using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Options;
using Dfe.Data.SearchPrototype.Common.CleanArchitecture.Application.UseCase;
using Dfe.Data.SearchPrototype.SearchForEstablishments;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.UseCase;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.SearchServiceAdapter;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.SearchServiceAdapter.Options;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.SearchServiceAdapter.Resources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Dfe.Data.SearchPrototype.Web.Tests;

public sealed class PageWebApplicationFactoryMockUseCase : WebApplicationFactory<Program>
{

    public static readonly IConfiguration TestConfiguration =
        new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.test.json", false)
                    .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // TODO temp fix to read and set application settings from test appsettings.json, until devs generate appsettings with secrets in
        if (
            string.IsNullOrEmpty(TestConfiguration["web:domain"]) ||
            string.IsNullOrEmpty(TestConfiguration["web:port"]) ||
            string.IsNullOrEmpty(TestConfiguration["web:scheme"]))
        {
            throw new ArgumentNullException("Missing test configuration: configure your user secrets file");
        };
        builder.ConfigureServices(services =>
        {
            // remove any services that need overriding with test configuration
            services.RemoveAll<IOptions<SearchByKeywordClientOptions>>();

            services.RemoveAll<IUseCase<SearchByKeywordRequest, SearchByKeywordResponse>>();

            // register dependencies with test configuration

            services.AddSingleton<IJsonFileLoader, JsonFileLoader>();
            
            services.AddScoped<IUseCase<SearchByKeywordRequest, SearchByKeywordResponse>, DummyUseCase>();

            //services.AddOptions<DummySearchServiceAdapterOptions>().Configure(
            //    (options) => options.FileName = TestConfiguration["dummySearchServiceAdapter:fileName"]);

            services.AddOptions<SearchByKeywordClientOptions>().Configure(
                (options) => options.Credentials = TestConfiguration["azureSearchClientOptions:credentials"]);

        });
    }
}