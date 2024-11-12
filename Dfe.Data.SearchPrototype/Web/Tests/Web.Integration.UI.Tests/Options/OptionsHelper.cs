using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using static Microsoft.Extensions.Options.Options;

namespace Dfe.Data.SearchPrototype.Web.Tests.Web.Integration.UI.Tests.Options;

public static class OptionsHelper
{
    private static readonly IConfiguration Configuration =
        new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.test.json", false)
                    .Build();

    public static IOptions<T> GetOptions<T>(string key) where T : class
        => Create(Configuration.GetRequiredSection(key)
            .Get<T>()
                ?? throw new ArgumentException("Configuration section returned null object"));
}
