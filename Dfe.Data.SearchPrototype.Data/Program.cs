using Dfe.Data.SearchPrototype.Data;
using Dfe.Data.SearchPrototype.Data.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

class Program
{
    static async Task Main(string[] args)
    {
        //configuration
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        builder.Configuration.Sources.Clear();

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddUserSecrets<Program>();

        AzureSearchServiceDetails searchDetails = new();
        builder.Configuration.GetRequiredSection(nameof(AzureSearchServiceDetails)).Bind(searchDetails);
        builder.Services.AddHttpClient("AzureSearchServiceAPI", httpClient =>
        {
            httpClient.BaseAddress = new Uri($"https://{searchDetails.ServiceName}.search.windows.net/indexes/{searchDetails.IndexName}/docs/index?api-version=2020-06-30");
        });
        builder.Services.AddHttpClient("PostcodeLookupAPI", httpClient =>
        {
            var clientUri = builder.Configuration.GetValue<string>("PostCodeAPIEndpoint");
            httpClient.BaseAddress = new Uri(clientUri!);
        });
        builder.Services.AddScoped<PostcodeLookupService>();

        using IHost host = builder.Build();

        var loggerFactory = LoggerFactory.Create(
            builder => builder.AddConsole());

        //run app
        var dataManager = new ManageData(host.Services.GetRequiredService<PostcodeLookupService>(), loggerFactory.CreateLogger<Program>());
        await dataManager.ExtractAndUploadData(searchDetails, builder.Configuration["filePath"]!);
    }
}






