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

    [Fact]
    public async Task SearchEstablishments()
    {
        // all searches have to have the '*' added for now - this needs to go to the top of the list for refactor
        var queryUrl = "/establishments?SearchKeyword=School*";
        var response = await _client.GetAsync(uri+queryUrl);

        response.EnsureSuccessStatusCode();
        var results = response.Content.ReadAsStringAsync();

        // test whatever you want to about the results here

    }
}
