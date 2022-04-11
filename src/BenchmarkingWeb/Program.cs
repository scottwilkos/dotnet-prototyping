using BenchmarkDotNet.Running;
using BenchmarkingWeb;

class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run<BenchmarkHarness>();
        Console.ReadKey();
    }
}