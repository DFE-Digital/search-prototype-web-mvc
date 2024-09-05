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
builder.Services.AddCognitiveSearchAdaptorServices();
builder.Services.AddSearchForEstablishmentServices();

builder.Services.AddOptions<SearchSettingsOptions>("establishments")
    .Configure<IConfiguration>(
        (settings, configuration) =>
            configuration.GetRequiredSection("SearchEstablishment:SearchSettingsOptions").Bind(settings));
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
