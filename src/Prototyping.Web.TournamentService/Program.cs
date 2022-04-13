using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Prototyping.Business.Cqrs;
using Prototyping.Domain;
using Prototyping.Domain.Helpers;
using Prototyping.Domain.Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Setup Mongo
ConfigureMongoDb(builder);

// Configure SQLite
ConfigureSqlite(builder);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddMediatR(typeof(AddTournamentHandler));
builder.Services.AddScoped<ITournamentRepository, TournamentEFRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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