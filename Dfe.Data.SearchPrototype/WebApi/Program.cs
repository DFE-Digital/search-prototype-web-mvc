using Dfe.Data.Common.Infrastructure.CognitiveSearch;
using Dfe.Data.SearchPrototype.Infrastructure;
using Dfe.Data.SearchPrototype.SearchForEstablishments;


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
builder.Services.AddCognitiveSearchAdaptorServices(builder.Configuration);
builder.Services.AddSearchForEstablishmentServices();

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
