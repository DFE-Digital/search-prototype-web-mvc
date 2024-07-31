using Dfe.Data.SearchPrototype.SearchForEstablishments;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.SearchServiceAdapter;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.SearchServiceAdapter.Options;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.SearchServiceAdapter.Resources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dfe.Data.SearchPrototype.Web.Tests.AcceptanceTests
{
    public sealed class WebApplicationFactoryFixture<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        public string HostUrl { get; set; } = "http://localhost:5000"; // Default.

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseUrls(HostUrl);
            builder.ConfigureServices(services => ConfigureServices(services));
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            IHost testHost = builder.Build();

            builder.ConfigureWebHost(webHostBuilder => webHostBuilder.UseKestrel());

            var host = builder.Build();
            host.Start();

            return testHost;
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Remove registration of the default ISearchServiceAdapter (i.e. CognitiveSearchServiceAdapter).
            var searchServiceAdapterDescriptor =
                    services.SingleOrDefault(
                        serviceDescriptor => serviceDescriptor.ServiceType ==
                            typeof(ISearchServiceAdapter));

            services.Remove(searchServiceAdapterDescriptor!);

            // Register our dummy search service adapter.
            services.AddSingleton<IJsonFileLoader, JsonFileLoader>();
            services.AddScoped(
                typeof(ISearchServiceAdapter),
                typeof(DummySearchServiceAdapter<Infrastructure.Establishment>));

            string fileName =
                new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.test.json", false)
                                .Build()["dummySearchServiceAdapter:fileName"]!;

            services.AddOptions<DummySearchServiceAdapterOptions>()
                .Configure((options) =>
                    options.FileName = fileName);
        }
    }
}