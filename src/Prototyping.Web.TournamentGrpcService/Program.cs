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
var client = new MongoClient("mongodb://localhost:27017");
var database = client.GetDatabase("TournamentsDatabase");
database.DropCollection("Tournaments");

// Configure SQLite
string dbFile = "TestDatabase.db";
SetupHelper.SetupDb(dbFile, Assembly.GetExecutingAssembly().GetName().Name);

// Configure MediatR and CQRS services for SQLite
//builder.Services.AddDbContext<TournamentContext>(o => o.UseSqlite($"Filename={dbFile}", options => { options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName); }));
builder.Services.AddDbContextPool<TournamentContext>(o => o.UseSqlite($"Filename={dbFile}", options => { options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName); }));

builder.Services.AddMediatR(typeof(AddTournamentHandler));
builder.Services.AddScoped<ITournamentRepository, TournamentEFRepository>();

// Add services to the container.
builder.Services.AddGrpc(_ => {
    _.MaxReceiveMessageSize = null;
    _.MaxSendMessageSize = null;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<TournamentService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
