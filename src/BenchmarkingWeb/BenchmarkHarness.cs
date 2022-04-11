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
                await _restClient.PostSampleTournamentPayloadAsync();
            }
        }
        
        [GlobalSetup(Targets = new[] {nameof(GetSampleTournamentPayloadAsync), nameof(GetSampleTournamentPayloadNoTrackingAsync)})]
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
                await _restClient.GetSampleTournamentPayloadNoTrackingAsync(id);
            }
        }
    }
} 
