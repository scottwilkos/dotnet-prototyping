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
}