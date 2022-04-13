using Prototyping.Domain.Models;

namespace BenchmarkingWeb
{
    internal class TournamentDto: ITournament{
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
