using Xunit;
using Dfe.Data.SearchPrototype.Web.Tests;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using System.Text.Json.Serialization;
using FluentAssertions;

namespace Dfe.Data.SearchPrototype.WebApi.Tests.APITests;

public class SearchResults : IClassFixture<PageWebApplicationFactory<Program>>
{
    private const string SEARCHKEYWORD_ENDPOINT = "/establishments?SearchKeyword=";
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
        var response = await _client.GetAsync("/swagger/index.html");

        response.EnsureSuccessStatusCode();
    }

    [Theory]
    [InlineData("Academy", 2)]
    [InlineData("School", 1)]
    public async Task GET_Search_Returns_ExpectedNumber_Of_Results(string query, int resultsInt)
    {
        // all searches have to have the '*' added for now - this needs to go to the top of the list for refactor
        var response = await _client.GetAsync($"{SEARCHKEYWORD_ENDPOINT}{query}*");

        var responseBody = await response.Content.ReadAsStringAsync();
        var jsonString = JsonConvert.DeserializeObject<SearchResultsJson>(responseBody)!;
        
        jsonString.EstablishmentResults.Establishments.Should().HaveCount(resultsInt);
    }

    [Fact]
    public async Task GET_Search_Returns_EstablishmentData()
    {
        var response = await _client.GetAsync($"{SEARCHKEYWORD_ENDPOINT}Academy*");

        var responseBody = await response.Content.ReadAsStringAsync();
        var results = JsonConvert.DeserializeObject<SearchResultsJson>(responseBody)!;

        results.EstablishmentResults!.Establishments!.First().Urn.Should().Be("123456");
        results.EstablishmentResults!.Establishments!.First().Name.Should().Be("Goose Academy");
        //TODO: address
        //results.EstablishmentResults.Establishments.First().Address.Should().Be("123456");
        results.EstablishmentResults!.Establishments!.First().EstablishmentType.Should().Be("Academy");
        results.EstablishmentResults!.Establishments!.First().EstablishmentStatusName.Should().Be("Open");
        results.EstablishmentResults!.Establishments!.First().PhaseOfEducation.Should().Be("Secondary");
    }
}

public class SearchResultsJson
{
    [JsonPropertyName("establishmentResults")]
    public EstablishmentSearchResults? EstablishmentResults { get; set; }
}

public class EstablishmentSearchResults
{
    [JsonPropertyName("establishments")]
    public IEnumerable<Establishment>? Establishments { get; set; }
}