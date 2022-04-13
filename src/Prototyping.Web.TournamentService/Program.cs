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
var client = new MongoClient("mongodb://localhost:27017");
var database = client.GetDatabase("TournamentsDatabase");
database.DropCollection("Tournaments");

// Configure SQLite
string dbFile = "TestDatabase.db";
SetupHelper.SetupDb(dbFile, Assembly.GetExecutingAssembly().GetName().Name);

// Configure MediatR and CQRS services for SQLite
builder.Services.AddDbContextPool<TournamentContext>(o => o.UseSqlite($"Filename={dbFile}", options => { options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName); }));

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