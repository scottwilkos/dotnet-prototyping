using System.Security.Authentication;
using MediatR;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Prototyping.Business.Cqrs
{
    public class GetTournamentMongoHandler : IRequestHandler<GetTournamentMongoQuery, TournamentMongoDto>
    {
        private static MongoClient client = new MongoClient(new MongoClientSettings
        {
            Server = new MongoServerAddress("localhost", 27017),
            MaxConnectionPoolSize = 500
        });
        private static IMongoDatabase _database = null;
        private static IMongoCollection<TournamentMongoDto> _tournaments;

        public GetTournamentMongoHandler(IConfiguration configuration)
        {
            if (_database == null)
            {
                var mongoConfiguration = configuration.GetSection("MongoDb");

                var connectionString = mongoConfiguration["ConnectionString"];

                MongoClientSettings settings = MongoClientSettings.FromUrl(
                  new MongoUrl(connectionString)
                );
                settings.SslSettings =
                  new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
                var client = new MongoClient(settings);

                _database = client.GetDatabase("TournamentsDatabase");
                _tournaments = _database.GetCollection<TournamentMongoDto>("Tournaments");
            }
        }

        public async Task<TournamentMongoDto> Handle(GetTournamentMongoQuery request, CancellationToken cancellationToken)
        {
            return await _tournaments.Find(_ => _.Id.Equals(request.Id)).SingleOrDefaultAsync(cancellationToken);
        }
    }
}