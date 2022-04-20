using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BenchmarkingGprc
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

        [GlobalSetup(Targets = new[] { nameof(GetSampleTournamentPayloadAsync), nameof(LoadTestParallelRequests)})]
        public async Task GetSampleTournamentPayloadAsyncSetup()
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

        [Benchmark]
        public async Task PostSampleTournamentPayloadAsync()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                await _client.AddTournamentAsync();
            }
        }

        [Benchmark]
        public async Task GetSampleTournamentPayloadAsync()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                var id = _ids[randomGenerator.GetRandomInt(0, _maxCount)];
                await _client.GetTournamentAsync(id);
            }
        }

        [Benchmark]
        public async Task LoadTestParallelRequests()
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
                if(item.Id != item.Tournament.Id){
                    throw new Exception("Ids do not match");
                }
            }
        }
    }
}