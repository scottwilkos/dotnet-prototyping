using BenchmarkDotNet.Attributes;
using Prototyping.Common;

namespace BenchmarkingGprc.BenchmarkHarnesses
{
    [HtmlExporter]
    public class BenchmarkMongoHarness
    {
        [Params(100, 200, 300, 400, 500)]
        public int IterationCount;
        private static readonly RandomGenerator randomGenerator = new RandomGenerator();
        private static string[] _ids = null;
        private static string[] _mongoIds = null;
        private static int _maxCount;
        private static int _maxMongoCount;

        private static BenchmarkMongoClient _client = new BenchmarkMongoClient();

        [GlobalSetup(Targets = new[] { nameof(Grpc_Mongo_PostInSerial), nameof(Grpc_Mongo_GetInSerial), nameof(Grpc_Mongo_GetInParallel), nameof(Gprc_Mongo_PostInParallel) })]
        public async Task Grpc_Mongo_GlobalSetup()
        {
            try
            {
                if (_ids == null || !_ids.Any())
                {
                    _ids = (await _client.GetTournamentsAsync()).Select(_ => _.Id).ToArray();
                    _maxCount = _ids.Length;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [Benchmark, BenchmarkCategory("Grpc", "Mongo")]
        public async Task Grpc_Mongo_PostInSerial()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                await _client.AddTournamentAsync();
            }
        }

        [Benchmark, BenchmarkCategory("Grpc", "Mongo")]
        public async Task Grpc_Mongo_GetInSerial()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                var id = _ids[randomGenerator.GetRandomInt(0, _maxCount)];
                await _client.GetTournamentAsync(id);
            }
        }

        [Benchmark, BenchmarkCategory("Grpc", "Mongo")]
        public async Task Grpc_Mongo_GetInParallel()
        {
            //
            RandomGenerator randomGenerator = new RandomGenerator();

            var maxCount = _ids.Length;

            // This simulates a load test scenario where 250 parallel requests are made to the API
            var tasks = new List<Task<MongoResultsVerifier>>();
            for (int i = 0; i < IterationCount; i++)
            {
                // Get an id and make a request
                var id = _ids[randomGenerator.GetRandomInt(0, _maxCount)];
                tasks.Add(_client.GetTournamentAsync(id));
            }

            // Run all 300 tasks in parallel
            var result = await Task.WhenAll(tasks);
            foreach (var item in result)
            {
                if (item.Id != item.Tournament.Id)
                {
                    throw new Exception("Ids do not match");
                }
            }
        }

        [Benchmark, BenchmarkCategory("Grpc", "Mongo")]
        public async Task Gprc_Mongo_PostInParallel()
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < IterationCount; i++)
            {
                tasks.Add(_client.AddTournamentAsync());
            }
            await Task.WhenAll(tasks);
        }
    }
}