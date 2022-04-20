using MediatR;
using Microsoft.EntityFrameworkCore;

using MongoDB.Driver;
using Prototyping.Business.Cqrs;
using Prototyping.Domain;
using Prototyping.Domain.Helpers;
using Prototyping.Domain.Repositories;
using System.Reflection;
using System.Security.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Setup Mongo
ConfigureMongoDb(builder);

ConfigureDatabase(builder);

// Configure SQLite
//ConfigureSqlite(builder);

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

    var connectionString = configuration["ConnectionString"];
    MongoClientSettings settings = MongoClientSettings.FromUrl(
      new MongoUrl(connectionString)
    );
    settings.SslSettings =
      new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
    var client = new MongoClient(settings);
    
    var database = client.GetDatabase(configuration["DatabaseName"]);
    database.DropCollection(configuration["TournamentsCollection"]);
}

void ConfigureDatabase(WebApplicationBuilder builder)
{
    var configuration = builder.Configuration.GetValue("SqlDatabaseType", "SQLite");
    if (configuration == "Sqlite")
    {
        ConfigureSqlite(builder);
    }
    else
    {
        ConfigureSqlServer(builder);
    }
}

void ConfigureSqlServer(WebApplicationBuilder builder)
{
    var configuration = builder.Configuration.GetSection("SqlServer");

    // Configure MediatR and CQRS services for SQLite
    builder.Services.AddDbContext<TournamentContext>(o => o.UseSqlServer(configuration["ConnectionString"]));
}

static void ConfigureSqlite(WebApplicationBuilder builder)
{
    var configuration = builder.Configuration.GetSection("Sqlite");
    string dbFile = configuration["Filename"];
    SetupHelper.SetupDb(dbFile, Assembly.GetExecutingAssembly().GetName().Name);

    // Configure MediatR and CQRS services for SQLite
    builder.Services.AddDbContextPool<TournamentContext>(o => o.UseSqlite(configuration["ConnectionString"], options => { options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName); }));
}