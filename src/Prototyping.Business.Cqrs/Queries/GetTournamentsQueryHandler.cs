using MediatR;
using Microsoft.EntityFrameworkCore;
using Prototyping.Domain.Models;
using Prototyping.Domain.Repositories;

namespace Prototyping.Business.Cqrs.Queries
{
    public class GetTournamentsQueryHandler : IRequestHandler<GetTournamentsQuery, IList<Tournament>>
    {
        private readonly ITournamentRepository tournamentRepository;

        public GetTournamentsQueryHandler(ITournamentRepository tournamentRepository)
        {
            this.tournamentRepository = tournamentRepository;
        }

        public async Task<IList<Tournament>> Handle(GetTournamentsQuery request, CancellationToken cancellationToken) => await tournamentRepository.GetAll(cancellationToken).ToListAsync(cancellationToken);
    }
    public class GetTournamentRecordCountQueryHandler : IRequestHandler<GetTournamentRecordCountQuery, long>
    {
        private readonly ITournamentRepository tournamentRepository;

        public GetTournamentRecordCountQueryHandler(ITournamentRepository tournamentRepository)
        {
            this.tournamentRepository = tournamentRepository;
        }

        public async Task<long> Handle(GetTournamentRecordCountQuery request, CancellationToken cancellationToken) => await tournamentRepository.Count(cancellationToken);
    }
}