using Prototyping.Common;
using Prototyping.Domain.Models;

internal class TournamentHelper
{
    private static RandomGenerator randomGenerator = new RandomGenerator();

    internal static Tournament[] CreateTournaments(int recordsToCreate)
    {
        List<Tournament> tournaments = new List<Tournament>();
        
        for (int i = 0; i < recordsToCreate; i++)
        {
            Tournament tournament = new Tournament(randomGenerator.GetRandomString(randomGenerator.GetRandomInt(25, 50)), randomGenerator.GetRandomString(randomGenerator.GetRandomInt(100, 200)));
        }

        return tournaments.ToArray();

    }
}