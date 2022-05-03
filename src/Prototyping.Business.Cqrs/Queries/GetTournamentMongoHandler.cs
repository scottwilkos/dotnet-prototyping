using System.Security.Authentication;
using MediatR;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Prototyping.Business.Cqrs
{
    public class GetTournamentMongoHandler : IRequestHandler<GetTournamentMongoQuery, TournamentMongoDto>
    {
        private static MongoClient client;
        private static IMongoDatabase _database = null;
        private static IMongoCollection<TournamentMongoDto> _tournaments;

        public GetTournamentMongoHandler(IConfiguration configuration)
        {
            var mongoConnectionString = configuration.GetSection("MongoDb:ConnectionString").Value;

            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(mongoConnectionString)
            );
            settings.SslSettings =
              new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };

            client = client ?? new MongoClient(settings);

            if (_database == null)
            {
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