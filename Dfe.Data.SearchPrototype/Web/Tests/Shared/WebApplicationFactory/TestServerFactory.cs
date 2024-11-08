using Dfe.Data.SearchPrototype.Web;
using Dfe.Data.SearchPrototype.Web.Tests.Web.Integration.HTTP.Tests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DfE.Data.SearchPrototype.Web.Tests.Shared.WebApplicationFactory;
public class TestServerFactory : WebApplicationFactory<Program>
{
    private readonly IConfigureWebHostHandler _configureWebHostHandler;

    public TestServerFactory(IConfigureWebHostHandler configureWebHostHandler)
    {
        _configureWebHostHandler = configureWebHostHandler;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        _configureWebHostHandler.Handle(builder);
    }
}