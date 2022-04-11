using MediatR;
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
}