using Dfe.Data.Common.Infrastructure.CognitiveSearch;
using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.Infrastructure;
using Dfe.Data.SearchPrototype.SearchForEstablishments;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.WebApi.Controllers;
using Dfe.Data.SearchPrototype.WebApi.Mappers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Start of IOC container registrations
//
//
builder.Services.AddDefaultSearchFilterServices(builder.Configuration);
builder.Services.AddDefaultCognitiveSearchServices(builder.Configuration);
builder.Services.AddCognitiveSearchAdaptorServices(builder.Configuration);
builder.Services.AddSearchForEstablishmentServices(builder.Configuration);
builder.Services.AddSingleton<IMapper<SearchRequest, IList<FilterRequest>?>, SearchRequestToFilterRequestsMapper>();
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

namespace Dfe.Data.SearchPrototype.WebApi
{
    public partial class Program { }
}
