using BenchmarkDotNet.Running;
using BenchmarkingGprc;
using BenchmarkingGprc.BenchmarkHarnesses;

Console.WriteLine("Starting Benchmarking");
await DataLoader.LoadRecordsIfNoneExist();
BenchmarkRunner.Run(typeof(BenchmarkMongoHarness).Assembly);
await DisplayRecordCounts();

async static Task DisplayRecordCounts()
{
    var _restClient = new BenchmarkClient("https://localhost:7028");
    var _mongoRestClient = new BenchmarkMongoClient();

    var records = (await _restClient.GetTournamentsAsync()).Count();
    var mongoRecords = (await _mongoRestClient.GetTournamentsAsync()).Count();

    Console.WriteLine($"Records from SQL: {records}");
    Console.WriteLine($"Records from Mongo: {mongoRecords}");
}

public static class DataLoader
{
    public async static Task LoadRecordsIfNoneExist()
    {
        const int recordsToCreate = 10000;

        var _restClient = new BenchmarkClient("https://localhost:7028");
        var _mongoRestClient = new BenchmarkMongoClient();

        var records = (await _restClient.GetTournamentsAsync()).Count();
        var mongoRecords = (await _mongoRestClient.GetTournamentsAsync()).Count();

        var iterations = recordsToCreate / 500;

        if (records == 0)
        {
            Console.WriteLine($"Creating {recordsToCreate} records");
            for (int i = 0; i < iterations; i++)
            {
                List<Task> tasks = new List<Task>();
                for (int j = 0; j < 500; j++)
                {
                    tasks.Add(_restClient.AddTournamentAsync());
                }

                await Task.WhenAll(tasks);
                Console.Write($" - {i}");
            }
        }

        if (mongoRecords == 0)
        {
            Console.WriteLine($"");
            Console.WriteLine($"Creating {recordsToCreate} Mongo records");
            for (int i = 0; i < iterations; i++)
            {
                List<Task> tasks = new List<Task>();
                for (int j = 0; j < 500; j++)
                {
                    tasks.Add(_mongoRestClient.AddTournamentAsync());
                }

                await Task.WhenAll(tasks);
                Console.Write($" - {i}");
            }
        }
    }
}

public static class MyExtensions
{
    public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> items, int maxItems)
    {
        return items.Select((item, index) => new { item, index })
                    .GroupBy(x => x.index / maxItems)
                    .Select(g => g.Select(x => x.item));
    }

    public static IEnumerable<T> Batch2<T>(this IEnumerable<T> items, int skip, int take)
    {
        return items.Skip(skip).Take(take);
    }

}