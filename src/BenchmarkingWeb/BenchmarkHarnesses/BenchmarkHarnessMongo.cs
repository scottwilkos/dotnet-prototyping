using BenchmarkDotNet.Attributes;

namespace BenchmarkingWeb.BenchmarkHarnesses
{
    [HtmlExporter]
    public class BenchmarkHarnessMongo
    {
        [Params(100, 200, 300)]
        public int IterationCount;
        private readonly RestClient _restClient = new RestClient();
        private readonly MongoRestClient _mongoRestClient = new MongoRestClient();

        private static readonly RandomGenerator randomGenerator = new RandomGenerator();
        private static string[] _mongoIds = null;
        private static int _maxMongoCount;

        #region MongoDatabaseBenchmarks
        [Benchmark]
        public async Task PostSampleTournamentMongoPayloadAsync()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                var result = await _mongoRestClient.PostSampleTournamentPayloadAsync();
            }
        }

        [GlobalSetup(Targets = new[] { nameof(GetSampleMongoTournamentPayloadAsync), nameof(LoadTest500ParallelMongoRequests) })]
        public async Task GetSampleMongoTournamentPayloadAsyncSetup()
        {
            try
            {
                if (_mongoIds == null || !_mongoIds.Any())
                {
                    _mongoIds = (await _mongoRestClient.GetSampleTournamentPayloadAsync()).Select(_ => _.Id).ToArray();
                    _maxMongoCount = _mongoIds.Length;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [Benchmark]
        public async Task GetSampleMongoTournamentPayloadAsync()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                var id = _mongoIds[randomGenerator.GetRandomInt(0, _maxMongoCount)];
                var tournament = await _mongoRestClient.GetSampleTournamentPayloadAsync(id);
            }
        }


        [Benchmark]
        public async Task LoadTest500ParallelMongoRequests()
        {
            //
            RandomGenerator randomGenerator = new RandomGenerator();

            var maxCount = _mongoIds.Length;

            int MAX_ITERATIONS = 5;
            int MAX_PARALLEL_REQUESTS = 500;

            // This simulates a load test scenario where 500 parallel requests are made to the API
            for (var step = 1; step <= MAX_ITERATIONS; step++)
            {
                var tasks = new List<Task<TournamentDto>>();
                for (int i = 0; i < MAX_PARALLEL_REQUESTS; i++)
                {
                    // Get an id and make a request
                    var id = _mongoIds[randomGenerator.GetRandomInt(0, maxCount)];
                    tasks.Add(_mongoRestClient.GetSampleTournamentPayloadAsync(id));
                }

                // Run all 500 tasks in parallel
                var result = await Task.WhenAll(tasks);
            }
        }

        #endregion

        [GlobalCleanup]
        public async Task PostRun()
        {
            var records = (await _restClient.GetSampleTournamentPayloadAsync()).Select(_ => _.Id).ToArray();
            var mongoRecords = (await _mongoRestClient.GetSampleTournamentPayloadAsync()).Select(_ => _.Id).ToArray();

            Console.WriteLine($"Records from SQL: {records.Length}");
            Console.WriteLine($"Records from Mongo: {mongoRecords.Length}");
        }
    }
}
