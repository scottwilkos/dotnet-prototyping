using BenchmarkDotNet.Attributes;
using Prototyping.Common;
using Prototyping.Common.Dtos;

namespace BenchmarkingWeb.BenchmarkHarnesses
{
    [HtmlExporter]
    public class BenchmarkHarnessMongo
    {
        [Params(100, 200, 300, 400, 500)]
        public int IterationCount;
        private readonly RestClient _restClient = new RestClient();
        private readonly MongoRestClient _mongoRestClient = new MongoRestClient();

        private static readonly RandomGenerator randomGenerator = new RandomGenerator();
        private static string[] _mongoIds = null;
        private static int _maxMongoCount;

        #region MongoDatabaseBenchmarks

        [GlobalSetup(Targets = new[] {nameof(Mongo_GetInSerial), nameof(Mongo_GetInParallel) })]
        public async Task Mongo_GlobalSetup()
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
                throw;
            }
        }

        // [Benchmark, BenchmarkCategory("WebClient", "Mongo")]
        // public async Task Mongo_PostInParallel()
        // {
        //     List<Task> tasks = new List<Task>();
        //     for (int i = 0; i < IterationCount; i++)
        //     {
        //         tasks.Add(_mongoRestClient.PostSampleTournamentPayloadAsync());
        //     }
        //     await Task.WhenAll(tasks);
        // }

        [Benchmark, BenchmarkCategory("WebClient", "Mongo")]
        public async Task Mongo_GetInSerial()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                var id = _mongoIds[randomGenerator.GetRandomInt(0, _maxMongoCount)];
                var tournament = await _mongoRestClient.GetSampleTournamentPayloadAsync(id);
            }
        }

        [Benchmark, BenchmarkCategory("WebClient", "Mongo")]
        public async Task Mongo_GetInParallel()
        {
            //
            RandomGenerator randomGenerator = new RandomGenerator();

            var maxCount = _mongoIds.Length;

            // This simulates a load test scenario where 500 parallel requests are made to the API
            var tasks = new List<Task<TournamentDto>>();
            for (int i = 0; i < IterationCount; i++)
            {
                // Get an id and make a request
                var id = _mongoIds[randomGenerator.GetRandomInt(0, maxCount)];
                tasks.Add(_mongoRestClient.GetSampleTournamentPayloadAsync(id));
            }

            // Run all 500 tasks in parallel
            var result = await Task.WhenAll(tasks);
        }

        #endregion

        [GlobalCleanup]
        public async Task PostRun()
        {
            var records = (await _restClient.GetSampleTournamentPayloadAsync()).Count();
            var mongoRecords = (await _mongoRestClient.GetSampleTournamentPayloadAsync()).Count();

            Console.WriteLine($"Records from SQL: {records}");
            Console.WriteLine($"Records from Mongo: {mongoRecords}");
        }
    }
}
