using Xunit;
using Dfe.Data.SearchPrototype.Web.Tests;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using static Dfe.Data.SearchPrototype.Web.Tests.Shared.Helpers.ApiHelpers;

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
    [InlineData("Academy", 2)]
    [InlineData("School", 1)]
    public async Task GET_Search_Returns_ExpectedNumber_Of_Results(string query, int resultsInt)
    {
        // all searches have to have the '*' added for now - this needs to go to the top of the list for refactor
        var response = await _client.GetAsync($"{SEARCHKEYWORD_ENDPOINT}{query}*");

        var responseBody = await response.Content.ReadAsStringAsync();
        var jsonString = JsonConvert.DeserializeObject<EstablishmentResultsProperty>(responseBody)!;

        jsonString.EstablishmentResults!.Establishments.Should().HaveCount(resultsInt);
    }

    [Fact]
    public async Task GET_Search_Returns_EstablishmentData()
    {
        var response = await _client.GetAsync($"{SEARCHKEYWORD_ENDPOINT}Academy*");

        var responseBody = await response.Content.ReadAsStringAsync();
        var results = JsonConvert.DeserializeObject<EstablishmentResultsProperty>(responseBody)!;

        var firstUrn = results.EstablishmentResults!.Establishments!.First().Urn;
        firstUrn.Should().Be("123456");

        var firstName = results.EstablishmentResults!.Establishments!.First().Name;
        firstName.Should().Be("Goose Academy");

        var firstStreet = results.EstablishmentResults.Establishments!.First().Address.Street;
        firstStreet.Should().Be("Goose Street");

        var firstLocality = results.EstablishmentResults.Establishments!.First().Address.Locality;
        firstLocality.Should().Be("Goose Locality");

        var firstAddress3 = results.EstablishmentResults.Establishments!.First().Address.Address3;
        firstAddress3.Should().Be("Goose Address 3");

        var firstTown = results.EstablishmentResults.Establishments!.First().Address.Town;
        firstTown.Should().Be("Goose Town");

        var firstPostCode = results.EstablishmentResults.Establishments!.First().Address.Postcode;
        firstPostCode.Should().Be("GOO OSE");

        var firstType = results.EstablishmentResults!.Establishments!.First().EstablishmentType;
        firstType.Should().Be("Academy");

        var firstStatus = results.EstablishmentResults!.Establishments!.First().EstablishmentStatusName;
        firstStatus.Should().Be("Open");

        var firstPhase = results.EstablishmentResults!.Establishments!.First().PhaseOfEducation;
        firstPhase.Should().Be("Secondary");
    }

    [Fact]
    public async Task GET_Search_NoMatch_Returns_NoEstablishmentData()
    {
        var response = await _client.GetAsync($"{SEARCHKEYWORD_ENDPOINT}Antony*");

        var responseBody = await response.Content.ReadAsStringAsync();
        var results = JsonConvert.DeserializeObject<EstablishmentResultsProperty>(responseBody)!;

        results.EstablishmentResults!.Establishments.Should().HaveCount(0);
    }
    
    [Fact]
    public async Task GET_Search_SpecialCharacter_Returns_NoEstablishmentData()
    {
        var response = await _client.GetAsync($"{SEARCHKEYWORD_ENDPOINT}!*");

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
