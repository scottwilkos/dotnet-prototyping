using BenchmarkDotNet.Attributes;

namespace BenchmarkingWeb.BenchmarkHarnesses
{
    [HtmlExporter]
    public class BenchmarkHarness
    {
        [Params(100, 200, 300, 400, 500)]
        public int IterationCount;
        private readonly RestClient _restClient = new RestClient();

        private static readonly RandomGenerator randomGenerator = new RandomGenerator();
        private static string[] _ids = null;
        private static int _maxCount;

        #region DatabaseBenchmarks
        [GlobalSetup(Targets = new[] {nameof(Sqlite_PostInSerial), nameof(Sqlite_GetInSerial), nameof(Sqlite_GetInSerial_AsNoTracking), nameof(Sqlite_GetInParallel) })]
        public async Task Sqlite_GlobalSetup()
        {
            await DataLoader.LoadRecordsIfNoneExist();

            if (_ids == null || !_ids.Any())
            {
                _ids = (await _restClient.GetSampleTournamentPayloadAsync()).Select(_ => _.Id).ToArray();
                _maxCount = _ids.Length;
            }
        }

        [Benchmark, BenchmarkCategory("WebClient", "Sqlite")]
        public async Task Sqlite_PostInSerial()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                await _restClient.PostSampleTournamentPayloadAsync();
            }
        }

        [Benchmark, BenchmarkCategory("WebClient", "Sqlite")]
        public async Task Sqlite_GetInSerial()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                var id = _ids[randomGenerator.GetRandomInt(0, _maxCount)];
                await _restClient.GetSampleTournamentPayloadAsync(id);
            }
        }

        [Benchmark, BenchmarkCategory("WebClient", "Sqlite")]
        public async Task Sqlite_GetInSerial_AsNoTracking()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                var id = _ids[randomGenerator.GetRandomInt(0, _maxCount)];
                var tournament = await _restClient.GetSampleTournamentPayloadNoTrackingAsync(id);
            }
        }

        [Benchmark, BenchmarkCategory("WebClient", "Sqlite")]
        public async Task Sqlite_GetInParallel()
        {
            //
            RandomGenerator randomGenerator = new RandomGenerator();

            var maxCount = _ids.Length;
            // This simulates a load test scenario where 500 parallel requests are made to the API
                var tasks = new List<Task<TournamentDto>>();
                for (int i = 0; i < IterationCount; i++)
                {
                    // Get an id and make a request
                    var id = _ids[randomGenerator.GetRandomInt(0, maxCount)];
                    tasks.Add(_restClient.GetSampleTournamentPayloadAsync(id));
                }

                // Run all  tasks in parallel
                var result = await Task.WhenAll(tasks);
        }
        #endregion
    }
}
