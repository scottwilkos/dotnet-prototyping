using MediatR;
using MongoDB.Driver;
using Prototyping.Domain.Models;

namespace Prototyping.Business.Cqrs
{
    public class AddTournamentMongoHandler : IRequestHandler<AddTournamentMongoCommand, ITournament>
    {
        private static MongoClient client = new MongoClient("mongodb://localhost:27017");
        private static IMongoDatabase _database = null;
        private static IMongoCollection<TournamentMongoDto> _tournaments;

        public AddTournamentMongoHandler()
        {
            if (_database == null)
            {
                _database = client.GetDatabase("TournamentsDatabase");
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
            await _tournaments.InsertOneAsync(tournament);

            return tournament;
        }
    }
}