using MediatR;
using MongoDB.Driver;

namespace Prototyping.Business.Cqrs
{
    public class GetTournamentMongoHandler : IRequestHandler<GetTournamentMongoQuery, TournamentMongoDto>
    {
        private static MongoClient client = new MongoClient(new MongoClientSettings{
            Server = new MongoServerAddress("localhost", 27017),
            MaxConnectionPoolSize = 500
            , WaitQueueSize = 2000
        });
        private static IMongoDatabase _database = null;
        private static IMongoCollection<TournamentMongoDto> _tournaments;

        public GetTournamentMongoHandler()
        {
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