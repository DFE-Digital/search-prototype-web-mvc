using Azure.Search.Documents.Models;
using Azure.Search.Documents;
using Azure;
using Dfe.Data.Common.Infrastructure.CognitiveSearch;
using Dfe.Data.SearchPrototype.Common.CleanArchitecture.Application.UseCase;
using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.Infrastructure.Mappers;
using Dfe.Data.SearchPrototype.Infrastructure.Options.Mappers;
using Dfe.Data.SearchPrototype.Infrastructure.Options;
using Dfe.Data.SearchPrototype.Infrastructure;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;
using Dfe.Data.SearchPrototype.SearchForEstablishments;
using Infrastructure = Dfe.Data.SearchPrototype.Infrastructure;
using Models = Dfe.Data.SearchPrototype.SearchForEstablishments.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Start of IOC container registrations
//
//
builder.Services.AddDefaultCognitiveSearchServices(builder.Configuration);
builder.Services.AddScoped(typeof(ISearchServiceAdapter), typeof(CognitiveSearchServiceAdapter<Infrastructure.Establishment>));
builder.Services.AddScoped<IUseCase<SearchByKeywordRequest, SearchByKeywordResponse>, SearchByKeywordUseCase>();
builder.Services.AddSingleton(typeof(IMapper<Response<SearchResults<Infrastructure.Establishment>>, EstablishmentResults>), typeof(AzureSearchResponseToEstablishmentResultMapper));
builder.Services.AddSingleton<IMapper<SearchSettingsOptions, SearchOptions>, SearchOptionsToAzureOptionsMapper>();
builder.Services.AddSingleton<IMapper<Infrastructure.Establishment, Address>, AzureSearchResultToAddressMapper>();
builder.Services.AddSingleton<IMapper<Infrastructure.Establishment, Models.Establishment>, AzureSearchResultToEstablishmentMapper>();
builder.Services.AddSingleton<IMapper<EstablishmentResults, SearchByKeywordResponse>, ResultsToResponseMapper>();
builder.Services.AddOptions<SearchSettingsOptions>("establishments")
    .Configure<IConfiguration>(
        (settings, configuration) =>
            configuration.GetRequiredSection("SearchEstablishment:SearchSettingsOptions").Bind(settings));
builder.Services.AddScoped<ISearchOptionsFactory, SearchOptionsFactory>();
//
//
// End of IOC container registrations

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
