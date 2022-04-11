using MediatR;
using Prototyping.Domain.Models;
using Prototyping.Domain.Repositories;

namespace Prototyping.Business.Cqrs
{
    public class AddTournamentHandler : IRequestHandler<AddTournamentCommand, Tournament>
    {
        private readonly ITournamentRepository repository;

        public AddTournamentHandler(ITournamentRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Tournament> Handle(AddTournamentCommand command, CancellationToken cancellationToken)
        {
            Tournament tournament = new Tournament(command.Name, command.Description);

            var results = await this.repository.Add(tournament, cancellationToken);

            await this.repository.Save(cancellationToken);

            return results.Entity;
        }
    }
}