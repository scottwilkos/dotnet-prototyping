using BenchmarkDotNet.Attributes;
using Prototyping.Domain.Models;

namespace BenchmarkingWeb
{
    [HtmlExporter]
    public class BenchmarkHarness
    {
        [Params(100, 200, 300)]
        public int IterationCount;
        private readonly RestClient _restClient = new RestClient();
        private readonly MongoRestClient _mongoRestClient = new MongoRestClient();

        private static readonly RandomGenerator randomGenerator = new RandomGenerator();
        private static string[] _ids = null;
        private static string[] _mongoIds = null;
        private static int _maxCount;
        private static int _maxMongoCount;

        #region DatabaseBenchmarks
        [GlobalSetup(Targets = new[] { nameof(GetSampleTournamentPayloadAsync), nameof(GetSampleTournamentPayloadNoTrackingAsync), nameof(LoadTest500ParallelRequests) })]
        public async Task GetSampleTournamentPayloadAsyncSetup()
        {
            if (_ids == null || !_ids.Any())
            {
                _ids = (await _restClient.GetSampleTournamentPayloadAsync()).Select(_ => _.Id).ToArray();
                _maxCount = _ids.Length;
            }
        }

        [Benchmark]
        public async Task PostSampleTournamentPayloadAsync()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                var result = await _restClient.PostSampleTournamentPayloadAsync();
            }
        }

        [Benchmark]
        public async Task GetSampleTournamentPayloadAsync()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                var id = _ids[randomGenerator.GetRandomInt(0, _maxCount)];
                await _restClient.GetSampleTournamentPayloadAsync(id);
            }
        }

        [Benchmark]
        public async Task GetSampleTournamentPayloadNoTrackingAsync()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                var id = _ids[randomGenerator.GetRandomInt(0, _maxCount)];
                var tournament = await _restClient.GetSampleTournamentPayloadNoTrackingAsync(id);
            }
        }

        [Benchmark]
        public async Task LoadTest500ParallelRequests()
        {
            //
            RandomGenerator randomGenerator = new RandomGenerator();

            var maxCount = _ids.Length;

            int MAX_ITERATIONS = 5;
            int MAX_PARALLEL_REQUESTS = 500;

            // This simulates a load test scenario where 500 parallel requests are made to the API
            for (var step = 1; step < MAX_ITERATIONS; step++)
            {
                var tasks = new List<System.Threading.Tasks.Task<TournamentDto>>();
                for (int i = 0; i < MAX_PARALLEL_REQUESTS; i++)
                {
                    // Get an id and make a request
                    var id = _ids[randomGenerator.GetRandomInt(0, maxCount)];
                    tasks.Add(_restClient.GetSampleTournamentPayloadAsync(id));
                }

                // Run all 300 tasks in parallel
                var result = await System.Threading.Tasks.Task.WhenAll(tasks);
            }
        }
        #endregion
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
                var tasks = new List<System.Threading.Tasks.Task<TournamentDto>>();
                for (int i = 0; i < MAX_PARALLEL_REQUESTS; i++)
                {
                    // Get an id and make a request
                    var id = _mongoIds[randomGenerator.GetRandomInt(0, maxCount)];
                    tasks.Add(_mongoRestClient.GetSampleTournamentPayloadAsync(id));
                }

                // Run all 500 tasks in parallel
                var result = await System.Threading.Tasks.Task.WhenAll(tasks);
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
