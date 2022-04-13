using Grpc.Core;
using MediatR;
using Prototyping.Business.Cqrs;
using Prototyping.Business.Cqrs.Queries;

namespace Prototyping.Web.TournamentGrpcService.Services
{
    public class TournamentService : TournamentGrpcService.TournamentService.TournamentServiceBase
    {
        private readonly IMediator _mediator;

        public TournamentService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<Tournament> AddTournament(AddTournamentRequest request, ServerCallContext context)
        {
            AddTournamentCommand command = new AddTournamentCommand { Name = request.Name, Description = request.Description };
            var results = await _mediator.Send(command);
            return new Tournament { Name = results.Name, Id = results.Id, Description = results.Description };
        }

        public override async Task<GetTournamentsResponse> GetTournaments(GetTournamentsRequest request, ServerCallContext context)
        {
            var command = new GetTournamentsQuery();
            var results = await _mediator.Send(command);

            var records = results.Select(_ => new Tournament
            {
                Id = _.Id,
                Name = _.Name,  
                Description = _.Description
            }).ToList();
            var tournamentsResponse = new GetTournamentsResponse();
            tournamentsResponse.Tournaments.AddRange(records);
            return tournamentsResponse;
        }

        public override async Task<GetTournamentResponse> GetTournament(GetTournamentRequest request, ServerCallContext context)
        {
            var command = new GetTournamentQuery { Id = request.Id };
            var results = await _mediator.Send(command);

            return new GetTournamentResponse { Tournament = new Tournament { Id = results.Id, Name = results.Name, Description = results.Description} };
        }
    }
}