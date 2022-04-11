using Prototyping.Domain.Models;

namespace Prototyping.Domain.Repositories
{
    public class TournamentEFRepository : RepositoryEntityFrameworkBase<Tournament>, ITournamentRepository
    {
        public TournamentEFRepository(TournamentContext context) : base(context)
        {
        }
    }
}