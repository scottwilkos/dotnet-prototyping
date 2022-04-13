using BenchmarkDotNet.Running;
using BenchmarkingWeb.BenchmarkHarnesses;

class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run(typeof(BenchmarkHarness).Assembly);
    }
}