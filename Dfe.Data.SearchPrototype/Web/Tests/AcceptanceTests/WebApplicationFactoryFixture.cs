using Dfe.Data.SearchPrototype.SearchForEstablishments;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.SearchServiceAdapter;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.SearchServiceAdapter.Options;
using Dfe.Data.SearchPrototype.Web.Tests.Shared.SearchServiceAdapter.Resources;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dfe.Data.SearchPrototype.Web.Tests.AcceptanceTests
{
    public class WebApplicationFactoryFixture<TEntryPoint> : WebApplicationFactory<TEntryPoint>
        where TEntryPoint : class
    {
        private const string _LocalhostBaseAddress = "https://localhost";
        private IWebHost _host;

        public string RootUri { get; private set; }

        public WebApplicationFactoryFixture()
        {
            ClientOptions.BaseAddress = new Uri(_LocalhostBaseAddress);
            // Breaking change while migrating from 2.2 to 3.1, TestServer was not called anymore
            CreateServer(CreateWebHostBuilder());
        }

        protected override TestServer CreateServer(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services => ConfigureServices(services));
            _host = builder.Build();
            _host.Start();
            RootUri = _host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.LastOrDefault();
            // not used but needed in the CreateServer method logic
            return new TestServer(new WebHostBuilder().UseStartup<TEntryPoint>());
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            var builder = WebHost.CreateDefaultBuilder(Array.Empty<string>());
            builder.UseStartup<TEntryPoint>();
            return builder;
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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _host?.Dispose();
            }
        }
    }

    //public sealed class WebApplicationFactoryFixture<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    //{
    //    public string HostUrl { get; set; } = "http://127.0.0.1:80"; // Default.

    //    protected override void ConfigureWebHost(IWebHostBuilder builder)
    //    {
    //        builder.UseUrls(HostUrl);
    //        builder.ConfigureServices(services => ConfigureServices(services));
    //    }

    //    protected override IHost CreateHost(IHostBuilder builder)
    //    {
    //        IHost testHost = builder.Build();

    //        builder.ConfigureWebHost(webHostBuilder => webHostBuilder.UseKestrel());

    //        var host = builder.Build();
    //        host.Start();

    //        return testHost;
    //    }

    //    private void ConfigureServices(IServiceCollection services)
    //    {
    //        // Remove registration of the default ISearchServiceAdapter (i.e. CognitiveSearchServiceAdapter).
    //        var searchServiceAdapterDescriptor =
    //                services.SingleOrDefault(
    //                    serviceDescriptor => serviceDescriptor.ServiceType ==
    //                        typeof(ISearchServiceAdapter));

    //        services.Remove(searchServiceAdapterDescriptor!);

    //        // Register our dummy search service adapter.
    //        services.AddSingleton<IJsonFileLoader, JsonFileLoader>();
    //        services.AddScoped(
    //            typeof(ISearchServiceAdapter),
    //            typeof(DummySearchServiceAdapter<Infrastructure.Establishment>));

    //        string fileName =
    //            new ConfigurationBuilder()
    //                    .SetBasePath(Directory.GetCurrentDirectory())
    //                        .AddJsonFile("appsettings.test.json", false)
    //                            .Build()["dummySearchServiceAdapter:fileName"]!;

    //        services.AddOptions<DummySearchServiceAdapterOptions>()
    //            .Configure((options) =>
    //                options.FileName = fileName);
    //    }
    //}
}