using MediatR;
using MongoDB.Driver;

namespace Prototyping.Business.Cqrs
{
    public class GetTournamentsMongoHandler : IRequestHandler<GetTournamentsMongoQuery, IList<TournamentMongoDto>>
    {

        private static MongoClient client = new MongoClient("mongodb://localhost:27017");
        private static IMongoDatabase _database = null;
        private static IMongoCollection<TournamentMongoDto> _tournaments;

        public GetTournamentsMongoHandler()
        {
            if (_database == null)
            {
                _database = client.GetDatabase("TournamentsDatabase");
                _tournaments = _database.GetCollection<TournamentMongoDto>("Tournaments");
            }
        }

        public async Task<IList<TournamentMongoDto>> Handle(GetTournamentsMongoQuery request, CancellationToken cancellationToken)
        {
            return await _tournaments.Find(t => true).ToListAsync();
        }
    }
}