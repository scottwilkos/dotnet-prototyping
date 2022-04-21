using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Protottyping.Business.SetupService;
using Prototyping.Business.Cqrs;
using Prototyping.Domain;
using Prototyping.Domain.Helpers;
using Prototyping.Domain.Repositories;
using Prototyping.Web.Mongo.TournamentGrpcService.Services;
using Prototyping.Web.TournamentGrpcService.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var databaseSetupService = new DatabaseSetupService();

Console.WriteLine("Starting Service");

databaseSetupService.ConfigureSql(builder);

builder.Services.AddMediatR(typeof(AddTournamentHandler));
builder.Services.AddScoped<ITournamentRepository, TournamentEFRepository>();

// Add services to the container.
builder.Services.AddGrpc(_ => {
    _.MaxSendMessageSize = null;
});

var app = builder.Build();

var clearDatabases = builder.Configuration.GetValue("ClearDatabases", "false");

var servicesProvider = app.Services.GetService<IServiceProvider>() ?? throw new Exception("Could not get service provider");
using (var scope = servicesProvider.CreateScope() ?? throw new NullReferenceException("Could not create scope"))
{
    var scopedProvider = scope.ServiceProvider.GetRequiredService<IServiceProvider>();
    if (clearDatabases == "true")
    {
        Console.WriteLine("Clearing databases...");
        databaseSetupService.ClearMongoDb(builder);
        await databaseSetupService.ClearSqlDatabase(builder, scopedProvider);
    }
    await databaseSetupService.CreateAndLoadDatabase(builder, scopedProvider);
}

// Configure the HTTP request pipeline.
app.MapGrpcService<TournamentService>();
app.MapGrpcService<TournamentMongoService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

Console.WriteLine("Ready for Requests");

app.Run();