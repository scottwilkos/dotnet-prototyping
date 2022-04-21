using MediatR;
using Microsoft.Extensions.Configuration;
using Prototyping.Business.Cqrs;
using Prototyping.Business.Cqrs.Queries;
using Prototyping.Common;

namespace Protottyping.Business.SetupService
{
    public class DatabaseSetupService
    {
        public async Task CreateAndLoadDatabase(WebApplicationBuilder app, IServiceProvider servicesProvider)
        {
            Console.WriteLine("Configuring databases...");
            Console.WriteLine("Wait until completed before starting benchmarks...");

            var loadData = app.Configuration.GetValue("LoadData", "ifEmpty");

            Console.WriteLine("Loading MongoDb");
            await LoadMongoDb(app, loadData, servicesProvider);

            Console.WriteLine("Loading Sql Database");
            await LoadSqlDb(app, loadData, servicesProvider);

        }

        public async Task LoadSqlDb(WebApplicationBuilder app, string loadData, IServiceProvider servicesProvider)
        {
            const int RecordsToCreate = 10000;
            const int BatchSize = 500;
            var configuration = app.Configuration.GetValue("SqlDatabaseType", "SQLite");

            RandomGenerator randomGenerator = new RandomGenerator();
            var mediator = servicesProvider.GetService<IMediator>() ?? throw new Exception("Could not find Mediator");

            var tournamentsQuery = new GetTournamentsQuery();
            var results = await mediator.Send(tournamentsQuery);

            if (!results.Any() || loadData == "always")
            {
                for (int i = 0; i <= RecordsToCreate; i += BatchSize)
                {
                    List<Task> tasks = new List<Task>();
                    for (int batchAdd = 0; batchAdd < BatchSize; batchAdd++)
                    {
                        tasks.Add(mediator.Send(new AddTournamentCommand
                        {
                            Name = randomGenerator.GetRandomString(randomGenerator.GetRandomInt(25, 50)),
                            Description = randomGenerator.GetRandomString(randomGenerator.GetRandomInt(100, 200))
                        }));
                    }

                    await Task.WhenAll(tasks);
                }
            }
        }

        public async Task LoadMongoDb(WebApplicationBuilder app, string loadData, IServiceProvider servicesProvider)
        {
            const int RecordsToCreate = 10000;
            const int BatchSize = 500;

            RandomGenerator randomGenerator = new RandomGenerator();
            var mediator = servicesProvider.GetService<IMediator>() ?? throw new Exception("Could not find Mediator");

            var tournamentsQuery = new GetTournamentsMongoQuery();
            var results = await mediator.Send(tournamentsQuery);

            if (!results.Any() || loadData == "always")
            {
                for (int i = 0; i <= RecordsToCreate; i += BatchSize)
                {
                    List<Task> tasks = new List<Task>();
                    for (int batchAdd = 0; batchAdd < BatchSize; batchAdd++)
                    {
                        tasks.Add(mediator.Send(new AddTournamentMongoCommand
                        {
                            Name = randomGenerator.GetRandomString(randomGenerator.GetRandomInt(25, 50)),
                            Description = randomGenerator.GetRandomString(randomGenerator.GetRandomInt(100, 200))
                        }));
                    }

                    await Task.WhenAll(tasks);
                }
            }
        }

        public void ClearSqlDb(WebApplicationBuilder app)
        {
            var configuration = app.Configuration.GetSection("Sqlite");
            string dbFile = configuration["Filename"];
            SetupHelper.ClearDb(dbFile, Assembly.GetExecutingAssembly().GetName().Name);
        }

        public void ClearMongoDb(WebApplicationBuilder app)
        {
            var configuration = app.Configuration.GetSection("MongoDb");

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

        public void ConfigureSql(WebApplicationBuilder builder)
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

        public void ConfigureSqlServer(WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration.GetSection("SqlServer");

            // Configure MediatR and CQRS services for SQLite
            builder.Services.AddDbContext<TournamentContext>(o => o.UseSqlServer(configuration["ConnectionString"]));
        }

        public void ConfigureSqlite(WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration.GetSection("Sqlite");

            // Configure MediatR and CQRS services for SQLite
            builder.Services.AddDbContextPool<TournamentContext>(o => o.UseSqlite(configuration["ConnectionString"], options => { options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName); }));

            string dbFile = configuration["Filename"];
            SetupHelper.EnsureCreated(dbFile, Assembly.GetExecutingAssembly().GetName().Name);
        }
    }
}