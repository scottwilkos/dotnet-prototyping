using BenchmarkDotNet.Running;
using BenchmarkingWeb;
using Prototyping.Domain.Models;

class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run<BenchmarkHarness>();
        // System.Threading.Tasks.Task.Run(async () =>
        //     {
        //         await RunLoad();
        //     });
        Console.ReadKey();
    }

    public static async Task RunLoad()
    {
        int MAX_ITERATIONS = 100;
        int MAX_PARALLEL_REQUESTS = 500;
        int DELAY = 100;

        //
        RestClient _restClient = new RestClient();
        RandomGenerator randomGenerator = new RandomGenerator();

        for (int i = 0; i < 1000; i++)
        {
            await _restClient.PostSampleTournamentPayloadAsync();
        }

        var records = await _restClient.GetSampleTournamentPayloadAsync();
        var ids = records.Select(_ => _.Id).ToArray();
        var maxCount = ids.Length;


        for (var step = 1; step < MAX_ITERATIONS; step++)
        {
            Console.WriteLine($"Started iteration: {step}");
            var tasks = new List<System.Threading.Tasks.Task<TournamentDto>>();
            for (int i = 0; i < MAX_PARALLEL_REQUESTS; i++)
            {
                var id = ids[randomGenerator.GetRandomInt(0, maxCount)];
                Console.WriteLine($"Requesting tournament with id: {id}");
                tasks.Add(_restClient.GetSampleTournamentPayloadAsync(id));
            }
            // Run all 300 tasks in parallel
            var result = await System.Threading.Tasks.Task.WhenAll(tasks);
           
            Console.WriteLine($"Completed Iteration: {step} - Count: {result.Length}");

            // Some delay before new iteration
            await System.Threading.Tasks.Task.Delay(DELAY);
        }

        Console.ReadKey();
    }
}