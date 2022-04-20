using BenchmarkDotNet.Running;
using BenchmarkingWeb;
using BenchmarkingWeb.BenchmarkHarnesses;
using Prototyping.Common;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting Benchmarking");
        BenchmarkRunner.Run(typeof(BenchmarkHarnessMongo).Assembly);
        await DisplayRecordCounts();
    }

    private async static Task DisplayRecordCounts()
    {
        var _restClient = new RestClient();
        var _mongoRestClient = new MongoRestClient();

        var records = (await _restClient.GetSampleTournamentPayloadAsync()).Count();
        var mongoRecords = (await _mongoRestClient.GetSampleTournamentPayloadAsync()).Count();

        Console.WriteLine($"Records from SQL: {records}");
        Console.WriteLine($"Records from Mongo: {mongoRecords}");
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