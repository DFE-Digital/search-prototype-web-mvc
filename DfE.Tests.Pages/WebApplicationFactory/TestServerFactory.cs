namespace DfE.Tests.Pages.WebApplicationFactory;
internal class TestServerFactory<T> : WebApplicationFactory<T> where T : class
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