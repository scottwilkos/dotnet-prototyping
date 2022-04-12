using MediatR;
using Microsoft.EntityFrameworkCore;
using Prototyping.Domain;
using Prototyping.Domain.Models;
using Prototyping.Domain.Repositories;

namespace Prototyping.Business.Cqrs.Queries
{
    public class GetTournamentHandler : IRequestHandler<GetTournamentQuery, Tournament>
    {
        private readonly ITournamentRepository repository;

        public GetTournamentHandler(ITournamentRepository tournamentRepository)
        {
            repository = tournamentRepository;
        }

        public async Task<Tournament> Handle(GetTournamentQuery query, CancellationToken cancellationToken) => await repository.FirstOrDefault(_ => _.Id.Equals(query.Id), cancellationToken);
    }

    public class GetTournamentAsReadonlyQueryHandler : IRequestHandler<GetTournamentAsReadonlyQuery, Tournament>
    {
        private readonly TournamentContext context;

        public GetTournamentAsReadonlyQueryHandler(TournamentContext context)
        {
            this.context = context;
        }

        public Task<Tournament> Handle(GetTournamentAsReadonlyQuery request, CancellationToken cancellationToken)
        {
            return this.context.Tournaments.AsNoTracking().SingleOrDefaultAsync(_ => _.Id.Equals(request.Id), cancellationToken);
        }
    }
}