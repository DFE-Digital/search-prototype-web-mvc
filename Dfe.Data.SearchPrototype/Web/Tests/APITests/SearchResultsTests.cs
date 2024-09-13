using Xunit;
using Dfe.Data.SearchPrototype.Web.Tests;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Dfe.Data.SearchPrototype.WebApi.Tests.APITests;

public class SearchResults : IClassFixture<PageWebApplicationFactory<Program>>
{
    private const string uri = "http://localhost:80";
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _logger;
    private readonly PageWebApplicationFactory<Program> _factory;

    public SearchResults(PageWebApplicationFactory<Program> factory, ITestOutputHelper logger)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true
        });
        _factory = factory;
        _logger = logger;
    }

    [Fact]
    public async Task GetSwaggerPage()
    {
        var response = await _client.GetAsync(uri + "/swagger/index.html");

        response.EnsureSuccessStatusCode();
    }
}
