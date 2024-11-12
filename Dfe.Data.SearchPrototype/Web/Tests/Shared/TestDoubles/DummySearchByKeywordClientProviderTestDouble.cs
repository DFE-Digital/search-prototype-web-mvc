using Azure.Search.Documents.Models;
using Azure.Search.Documents;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers;
using Moq;
using Azure;
using Dfe.Data.SearchPrototype.Infrastructure.DataTransferObjects;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles;

public class DummySearchByKeywordClientProviderTestDouble : ISearchByKeywordClientProvider
{
    private readonly SearchResponseBuilder _builder;

    public DummySearchByKeywordClientProviderTestDouble(SearchResponseBuilder builder)
    {
        _builder = builder;
    }

    public Task<SearchClient> InvokeSearchClientAsync(string indexName)
    {
        var clientMock = new Mock<SearchClient>(()
            => new(
                new Uri("https://localhost"), indexName, new AzureKeyCredential("key")));

        clientMock.SetupGet(x => x.IndexName).Returns(indexName);

        clientMock.Setup(x => x.SearchAsync<Establishment>(
                It.IsAny<string>(),
                It.IsAny<SearchOptions>(), // TODO configure this?
                It.IsAny<CancellationToken>()
            )).Returns(
                Task.FromResult(
                    _builder.Build()));

        return Task.FromResult(clientMock.Object);
    }
}


public sealed class SearchResponseBuilder
{
    private readonly IEstablishmentBuilder _establishmentBuilder;
    private readonly List<Establishment> _establishments = [];

    public SearchResponseBuilder(IEstablishmentBuilder establishmentBuilder)
    {
        ArgumentNullException.ThrowIfNull(establishmentBuilder);
        _establishmentBuilder = establishmentBuilder;
    }

    public SearchResponseBuilder AddEstablishment(Action<IEstablishmentBuilder> configureBuilder)
    {
        configureBuilder(_establishmentBuilder);
        _establishments.Add(
            _establishmentBuilder.Build());
        return this;
    }

    public SearchResponseBuilder ClearEstablishments()
    {
        _establishments.Clear();
        return this;
    }

    // TODO public SearchResponseBuilder AddFacet()

    public Response<SearchResults<Establishment>> Build()
    {
        var responseMock = new Mock<Response>();

        IEnumerable<SearchResult<Establishment>> searchResults =
            _establishments.Select(
                (establishment) => SearchModelFactory.SearchResult(
                    document: establishment,
                    score: 0.9,
                    highlights: null));

        return Response.FromValue(
                SearchModelFactory.SearchResults(
                    searchResults,
                    searchResults.Count(),
                    facets: null,
                    null,
                    responseMock.Object),
                responseMock.Object);
    }
}

public interface IEstablishmentBuilder
{
    IEstablishmentBuilder SetName(string establishmentName);
    IEstablishmentBuilder SetTypeOfEstablishment(string typeOfEstablishment);
    IEstablishmentBuilder SetId(string id);
    IEstablishmentBuilder SetPhaseOfEducation(string phaseOfEducation);
    IEstablishmentBuilder SetStatus(string status);
    IEstablishmentBuilder SetAddress(string address);
    Establishment Build();
}

public sealed class EstablishmentBuilder : IEstablishmentBuilder
{
    private string? _establishmentName = null;
    private string? _id = null;
    private string? _typeOfEstablishmentName = null;
    private string? _phaseOfEducation = null;
    private string? _establishmentStatus = null;
    private string? _address = null;
    public EstablishmentBuilder()
    {
        
    }
    public Establishment Build()
        => new()
        {
            ESTABLISHMENTNAME = _establishmentName,
            id = _id,
            TYPEOFESTABLISHMENTNAME = _typeOfEstablishmentName,
            PHASEOFEDUCATION = _phaseOfEducation,
            ESTABLISHMENTSTATUSNAME = _establishmentStatus,
            
        };

    public IEstablishmentBuilder SetName(string establishmentName)
    {
        _establishmentName = establishmentName;
        return this;
    }
    public IEstablishmentBuilder SetStatus(string establishmentStatus)
    {
        _establishmentStatus = establishmentStatus;
        return this;
    }

    public IEstablishmentBuilder SetId(string id)
    {
        _id = id;
        return this;
    }

    public IEstablishmentBuilder SetPhaseOfEducation(string phaseOfEducation)
    {
        _phaseOfEducation = phaseOfEducation;
        return this;
    }

    public IEstablishmentBuilder SetTypeOfEstablishment(string typeOfEstablishmentName)
    {
        _typeOfEstablishmentName = typeOfEstablishmentName;
        return this;
    }

    public IEstablishmentBuilder SetAddress(string address)
    {
        _address = address;
        return this;
    }
}
