using BenchmarkDotNet.Running;
using BenchmarkingGprc;

BenchmarkRunner.Run<BenchmarkHarness>();

// var client = new BenchmarkClient();

// await client.AddTournamentAsync();

// var reply = await client.GetTournamentsAsync();

// if (reply?.Count != null)
// {
//     Console.WriteLine($"Tounrament Records: {reply.Count}");
// }
// else
// {
//     Console.WriteLine("Error");
// }

// Console.WriteLine("Exiting...");