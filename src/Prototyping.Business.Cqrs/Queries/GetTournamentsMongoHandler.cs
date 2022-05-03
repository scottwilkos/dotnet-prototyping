using System.Security.Authentication;
using MediatR;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Prototyping.Business.Cqrs
{
    public class GetTournamentsMongoHandler : IRequestHandler<GetTournamentsMongoQuery, IList<TournamentMongoDto>>
    {
        private static MongoClient client;
        private static IMongoDatabase _database;
        private static IMongoCollection<TournamentMongoDto> _tournaments;

        public GetTournamentsMongoHandler(IConfiguration configuration)
        {
            var mongoConnectionString = configuration.GetSection("MongoDb:ConnectionString").Value;

            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(mongoConnectionString)
            );
            settings.SslSettings =
              new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };

            client = client ?? new MongoClient(settings);
            _database = _database ?? client.GetDatabase("TournamentsDatabase");
            _tournaments = _tournaments ?? _database.GetCollection<TournamentMongoDto>("Tournaments");
        }

        public async Task<IList<TournamentMongoDto>> Handle(GetTournamentsMongoQuery request, CancellationToken cancellationToken)
        {
            return await _tournaments.Find(t => true).ToListAsync();
        }
    }
}