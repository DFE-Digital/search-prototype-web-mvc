using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.Net.NetworkInformation;

namespace Dfe.Data.SearchPrototype.Web.Tests.AcceptanceTests.Extensions
{
    /// <summary>
    /// Extension method used to establish the url the web host will listen to with an available port.
    /// </summary>
    public static class WebHostBuilderExtensions
    {
        /// <summary>
        /// Root extension method used to specify the url the web host will listen to.
        /// </summary>
        /// <returns>
        /// An integer value representing an available free IP port.
        /// </returns>
        public static void UseUrlsWithAvailablePort(this IWebHostBuilder builder)
        {
            int IpPort = 5000;

            if (!IsIpPortAvailable(IpPort)){
                IpPort = NextAvailableIpPort(IpPort);
            }

            builder.UseUrls(urls: $"http://localhost:{IpPort}");
        }

        /// <summary>
        /// Uses the IPGlobalProperties.GetActiveTcpListeners() method to test if a port is available,
        /// without trying to open it in advance. GetActiveTcpListeners() returns all active TCP listeners
        /// on the system, and is used to determine if a port is free or not.
        /// </summary>
        /// <param name="port">
        /// The initial port number to use for testing availability.
        /// </param>
        /// <returns>
        /// A boolean value representing the availability (true/false) of the requested port.
        /// </returns>
        private static bool IsIpPortAvailable(int port)
        {
            IPGlobalProperties properties =
                IPGlobalProperties.GetIPGlobalProperties();

            IPEndPoint[] listeners =
                properties.GetActiveTcpListeners();

            int[] openPorts =
                listeners.Select(IpEndPoint =>
                    IpEndPoint.Port).ToArray();

            return openPorts
                .All(openPort =>
                    openPort != port);
        }

        /// <summary>
        /// Starts with the default port (5000) and continue to increment until
        /// a free IP port is discovered using the IsFreeIpPort method.
        /// </summary>
        /// <param name="port">
        /// The initial port number to use for testing availability.
        /// </param>
        /// An integer value representing an available free IP port.
        private static int NextAvailableIpPort(int port = 5000)
        {
            port =
                (port > 0) ? port :
                    new Random().Next(1, 65535); // set the port range from 5001 to 65535

            while (!IsIpPortAvailable(port))
            {
                port++;
            }

            return port;
        }
    }
}
