using BenchmarkDotNet.Attributes;

namespace BenchmarkingGprc.BenchmarkHarnesses
{
    [HtmlExporter]
    public class BenchmarkHarness
    {
        [Params(100, 200, 300, 400, 500)]
        public int IterationCount;
        private static readonly RandomGenerator randomGenerator = new RandomGenerator();
        private static string[] _ids = null;
        private static string[] _mongoIds = null;
        private static int _maxCount;
        private static int _maxMongoCount;

        private static BenchmarkClient _client = new BenchmarkClient();

        [GlobalSetup(Targets = new[] { nameof(Grpc_Sqlite_GetInSerial), nameof(Grpc_Sqlite_GetInParallel) })]
        public async Task Grpc_Sqlite_GlobalSetup()
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

        [Benchmark, BenchmarkCategory("Grpc", "Sqlite")]
        public async Task Grpc_Sqlite_PostInSerial()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                await _client.AddTournamentAsync();
            }
        }

        [Benchmark, BenchmarkCategory("Grpc", "Sqlite")]
        public async Task Grpc_Sqlite_GetInSerial()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                var id = _ids[randomGenerator.GetRandomInt(0, _maxCount)];
                await _client.GetTournamentAsync(id);
            }
        }

        [Benchmark, BenchmarkCategory("Grpc", "Sqlite")]

        public async Task Grpc_Sqlite_GetInParallel()
        {
            //
            RandomGenerator randomGenerator = new RandomGenerator();

            var maxCount = _ids.Length;

            // This simulates a load test scenario where 250 parallel requests are made to the API
            var tasks = new List<Task<ResultsVerifier>>();
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
    }
}