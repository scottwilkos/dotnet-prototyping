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
        private static readonly RandomGenerator randomGenerator = new RandomGenerator();
        private static string[] _ids = null;
        private static int _maxCount;
    
        
        [Benchmark]
        public async Task PostSampleTournamentPayloadAsync()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                var result = await _restClient.PostSampleTournamentPayloadAsync();
            }
        }
        
        [GlobalSetup(Targets = new[] {nameof(GetSampleTournamentPayloadAsync), nameof(GetSampleTournamentPayloadNoTrackingAsync), nameof(LoadTest500ParallelRequests)})]
        public async Task GetSampleTournamentPayloadAsyncSetup()
        {
            if(_ids == null || !_ids.Any()){
                _ids = (await _restClient.GetSampleTournamentPayloadAsync()).Select(_ => _.Id).ToArray();
                _maxCount = _ids.Length;
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
                Console.WriteLine($"{tournament.Id} - {tournament.Name}");
            }
        }

        [Benchmark]
        public async Task LoadTest500ParallelRequests()
        {
            //
            RestClient _restClient = new RestClient();
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
            
                Console.WriteLine($"Completed Iteration: {step} - Count: {result.Length}");
            }
        }
    }
} 
