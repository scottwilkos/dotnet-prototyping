using System.Security.Authentication;
using MediatR;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Prototyping.Common.Dtos;

namespace Prototyping.Business.Cqrs
{
    public class AddTournamentMongoHandler : IRequestHandler<AddTournamentMongoCommand, ITournament>
    {
        private static MongoClient _client;
        private static IMongoDatabase _database;
        private static IMongoCollection<TournamentMongoDto> _tournaments;

        public AddTournamentMongoHandler(IConfiguration configuration)
        {
            var mongoConnectionString = configuration.GetSection("MongoDb:ConnectionString").Value;

            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(mongoConnectionString)
            );
            settings.SslSettings =
              new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };

            _client = _client ?? new MongoClient(settings);

            if (_database == null)
            {
                _database = _client.GetDatabase("TournamentsDatabase");
                _tournaments = _database.GetCollection<TournamentMongoDto>("Tournaments");
            }
        }

        public async Task<ITournament> Handle(AddTournamentMongoCommand command, CancellationToken cancellationToken)
        {
            var tournament = new TournamentMongoDto
            {
                Name = command.Name,
                Description = command.Description
            };
            await _tournaments.InsertOneAsync(tournament, cancellationToken: cancellationToken);

            return tournament;
        }
    }
}