using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.ServiceAdapters;
using Dfe.Data.SearchPrototype.Web.Tests.AcceptanceTests.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dfe.Data.SearchPrototype.Web.Tests.AcceptanceTests
{
    /// <summary>
    /// Factory for bootstrapping the web application in memory for functional end to end tests.
    /// </summary>
    /// <typeparam name="TEntryPoint">
    /// The entry point to use for invoking the system under test.
    /// </typeparam>
    public sealed class WebApplicationFactoryFixture<TEntryPoint> :
        WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        private IHost? _host;

        /// <summary>
        /// Gives the fixture an opportunity to configure the application before it gets built
        /// using the default service dependencies and with the prescribed web host url.
        /// </summary>
        /// <param name="builder">The <see cref="IWebHostBuilder"/> for the application.</param>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseUrlsWithAvailablePort();
            builder.ConfigureServices(services =>
                ConfigureServices(services));
        }

        /// <summary>
        /// Creates the <see cref="IHost"/> with the bootstrapped application in <paramref name="builder"/>.
        /// This is only called for applications using <see cref="IHostBuilder"/>. The builder sets-up the
        /// Kestrel web server to be used by the web host.
        /// </summary>
        /// <param name="builder">The <see cref="IHostBuilder"/> used to create the host.</param>
        /// <returns>The <see cref="IHost"/> with the bootstrapped application.</returns>
        protected override IHost CreateHost(IHostBuilder builder)
        {
            _host = builder.Build();

            builder.ConfigureWebHost(webHostBuilder =>
                webHostBuilder.UseKestrel());

            var host = builder.Build();
            host.Start();

            return _host;
        }

        /// <summary>
        /// Configures the additional service dependencies we want to
        /// override, notably the dummy search service adapter.
        /// </summary>
        /// <param name="services">
        /// The contract for a collection of application services.
        /// </param>
        private void ConfigureServices(IServiceCollection services) { }
     

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources, in this case the IHost instance.
        /// </summary>
        /// <param name="disposing">
        /// Flag to notify whether GC managed disposal is under way.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _host?.Dispose();
            }
        }
    }
}