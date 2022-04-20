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
        [GlobalSetup(Targets = new[] {nameof(PostSampleTournamentPayloadAsync), nameof(GetSampleTournamentPayloadAsync), nameof(GetSampleTournamentPayloadNoTrackingAsync), nameof(LoadTestParallelRequests) })]
        public async Task GetSampleTournamentPayloadAsyncSetup()
        {
            await DataLoader.LoadRecordsIfNoneExist();

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
                await _restClient.PostSampleTournamentPayloadAsync();
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
        public async Task LoadTestParallelRequests()
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
