using MediatR;
using Microsoft.EntityFrameworkCore;
using Prototyping.Business.Cqrs;
using Prototyping.Domain;
using Prototyping.Domain.Helpers;
using Prototyping.Domain.Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configure SQLite
string dbFile = "TestDatabase.db";
SetupHelper.SetupDb(dbFile, Assembly.GetExecutingAssembly().GetName().Name);
builder.Services.AddDbContext<TournamentContext>(o => o.UseSqlite($"Filename={dbFile}", options => { options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName); }));

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