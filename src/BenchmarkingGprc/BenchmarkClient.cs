using Grpc.Net.Client;
using Prototyping.Common;
using Prototyping.Web.TournamentGrpcService;

namespace BenchmarkingGprc
{
    public class BenchmarkClient
    {
        private static readonly GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:7028",
            new GrpcChannelOptions { MaxReceiveMessageSize = null });
        private static TournamentService.TournamentServiceClient? _client;

        private static RandomGenerator randomGenerator = new RandomGenerator();

        public BenchmarkClient()
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            _client = new TournamentService.TournamentServiceClient(channel);
        }

        public async Task AddTournamentAsync()
        {
            var request = new AddTournamentRequest { Name = randomGenerator.GetRandomString(randomGenerator.GetRandomInt(25, 50)), Description = randomGenerator.GetRandomString(randomGenerator.GetRandomInt(100, 200)) };
            var reply = await _client.AddTournamentAsync(request);
        }

        public async Task<List<Tournament>> GetTournamentsAsync()
        {
            var request = new GetTournamentsRequest();

            var reply = await _client.GetTournamentsAsync(request);

            return reply.Tournaments.ToList();
        }

        public async Task<ResultsVerifier> GetTournamentAsync(string id)
        {
            var request = new GetTournamentRequest() { Id = id };

            var reply = await _client.GetTournamentAsync(request);

            return new ResultsVerifier
            {
                Id = id,
                Tournament = reply.Tournament
            };
        }
    }

    public class ResultsVerifier
    {
        public string Id { get; set; }

        public Tournament Tournament { get; set; }
    }
}