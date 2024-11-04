using Xunit;
using Dfe.Data.SearchPrototype.Web.Tests;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using static Dfe.Data.SearchPrototype.Web.Tests.Shared.Helpers.ApiHelpers;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Dfe.Data.SearchPrototype.WebApi.Tests.APITests;

public class ApiSearchResults : IClassFixture<PageWebApplicationFactory<Program>>
{
    private const string SEARCHKEYWORD_ENDPOINT = "/establishments?SearchKeyword=";
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _logger;
    private readonly PageWebApplicationFactory<Program> _factory;

    public ApiSearchResults(PageWebApplicationFactory<Program> factory, ITestOutputHelper logger)
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
    [InlineData("Academy", 100)]
    [InlineData("School", 100)]
    public async Task GET_Search_Returns_ExpectedNumber_Of_Results(string query, int resultsInt)
    {
        var response = await _client.GetAsync($"{SEARCHKEYWORD_ENDPOINT}{query}");

        var responseBody = await response.Content.ReadAsStringAsync();
        var results = JsonConvert.DeserializeObject<EstablishmentResultsProperty>(responseBody)!;

        results.EstablishmentResults!.Establishments.Should().HaveCount(resultsInt);
    }

    [Fact]
    public async Task GET_Search_Returns_EstablishmentData()
    {
        var response = await _client.GetAsync($"{SEARCHKEYWORD_ENDPOINT}Academy");

        var responseBody = await response.Content.ReadAsStringAsync();
        var results = JsonConvert.DeserializeObject<EstablishmentResultsProperty>(responseBody)!;

        var firstEstablishmentResult = results.EstablishmentResults!.Establishments!.First();
        firstEstablishmentResult.Urn.Should().NotBeNull();
        firstEstablishmentResult.Name.Should().NotBeNull();
        firstEstablishmentResult.Address.Street.Should().NotBeNull();
        firstEstablishmentResult.Address.Locality.Should().NotBeNull();
        firstEstablishmentResult.Address.Address3.Should().NotBeNull();
        firstEstablishmentResult.Address.Town.Should().NotBeNull();
        firstEstablishmentResult.Address.Postcode.Should().NotBeNull();
        firstEstablishmentResult.EstablishmentType.Should().NotBeNull();
        firstEstablishmentResult.EstablishmentStatusName.Should().NotBeNull();
        firstEstablishmentResult.PhaseOfEducation.Should().NotBeNull();
    }

    [Theory]
    [InlineData("St")]
    [InlineData("Jos")]
    [InlineData("Cath")]
    public async Task GET_Search_ByPartialName_ReturnsResults(string query)
    {
        var response = await _client.GetAsync($"{SEARCHKEYWORD_ENDPOINT}{query}");

        var responseBody = await response.Content.ReadAsStringAsync();
        var results = JsonConvert.DeserializeObject<EstablishmentResultsProperty>(responseBody)!;

        results.EstablishmentResults!.Establishments.Should().HaveCountGreaterThan(1);

        var establishmentResults = results.EstablishmentResults!.Establishments!;
        foreach (var headings in establishmentResults!)
        {
            establishmentResults!.First().Name.Should().Contain(query);
        }
    }

    [Theory]
    [InlineData("Catholic")]
    [InlineData("Junior")]
    public async Task GET_Search_Returns_Facets(string query)
    {
        var response = await _client.GetAsync($"{SEARCHKEYWORD_ENDPOINT}{query}");

        var responseBody = await response.Content.ReadAsStringAsync();
        var results = JsonConvert.DeserializeObject<EstablishmentFacetsProperty>(responseBody)!;

        var facets = results.EstablishmentFacetResults!.Facets!.Count();
        facets.Should().Be(2);

        var establishmentStatus = results.EstablishmentFacetResults!.Facets!.SelectMany(t => t.Name!);
        establishmentStatus.Should().Contain("PHASEOFEDUCATION");
        establishmentStatus.Should().Contain("ESTABLISHMENTSTATUS");
    }

    [Theory]
    [InlineData("zzz")]
    [InlineData("123")]
    public async Task GET_Search_NoMatch_Returns_NoEstablishmentData(string query)
    {
        var response = await _client.GetAsync($"{SEARCHKEYWORD_ENDPOINT}{query}");

        var responseBody = await response.Content.ReadAsStringAsync();
        var results = JsonConvert.DeserializeObject<EstablishmentResultsProperty>(responseBody)!;

        results.EstablishmentResults!.Establishments.Should().HaveCount(0);
    }
    
    [Fact]
    public async Task GET_Search_SpecialCharacter_Returns_NoEstablishmentData()
    {
        var response = await _client.GetAsync($"{SEARCHKEYWORD_ENDPOINT}!");

        var responseBody = await response.Content.ReadAsStringAsync();
        var results = JsonConvert.DeserializeObject<EstablishmentResultsProperty>(responseBody)!;

        results.EstablishmentResults!.Establishments.Should().HaveCount(0);
    }
    
    [Fact]
    public async Task GET_Search_NoSearchTerm_Returns_400()
    {
        var response = await _client.GetAsync($"{SEARCHKEYWORD_ENDPOINT}");

        response.StatusCode.Should().Be((System.Net.HttpStatusCode)StatusCodes.Status400BadRequest);
    }
}
