using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Prototyping.Business.Cqrs;
using Prototyping.Domain;
using Prototyping.Domain.Helpers;
using Prototyping.Domain.Repositories;
using Prototyping.Web.TournamentGrpcService.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Setup Mongo
ConfigureMongoDb(builder);

// Configure SQLite
ConfigureSqlite(builder);

builder.Services.AddMediatR(typeof(AddTournamentHandler));
builder.Services.AddScoped<ITournamentRepository, TournamentEFRepository>();

// Add services to the container.
builder.Services.AddGrpc(_ => {
    _.MaxSendMessageSize = null;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<TournamentService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

static void ConfigureMongoDb(WebApplicationBuilder builder)
{
    var configuration = builder.Configuration.GetSection("MongoDb");
    var client = new MongoClient(configuration["ConnectionString"]);
    var database = client.GetDatabase(configuration["DatabaseName"]);
    database.DropCollection(configuration["TournamentsCollection"]);
}

static void ConfigureSqlite(WebApplicationBuilder builder)
{
    var configuration = builder.Configuration.GetSection("Sqlite");
    string dbFile = configuration["Filename"];
    SetupHelper.SetupDb(dbFile, Assembly.GetExecutingAssembly().GetName().Name);

    // Configure MediatR and CQRS services for SQLite
    builder.Services.AddDbContextPool<TournamentContext>(o => o.UseSqlite(configuration["ConnectionString"], options => { options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName); }));
}