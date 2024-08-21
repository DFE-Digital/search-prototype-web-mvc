using Dfe.Data.SearchPrototype.Data;
using Dfe.Data.SearchPrototype.Data.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

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

        using IHost host = builder.Build();

        //run app
        await ManageData.ExtractAndUploadData(searchDetails, builder);
    }
}






