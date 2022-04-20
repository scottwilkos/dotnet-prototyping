using Grpc.Core;
using MediatR;
using Prototyping.Business.Cqrs;
using Prototyping.Business.Cqrs.Queries;

namespace Prototyping.Web.Mongo.TournamentGrpcService.Services
{
    public class TournamentMongoService : TournamentGrpcService.TournamentMongoService.TournamentMongoServiceBase
    {
        private readonly IMediator _mediator;

        public TournamentMongoService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<Tournament> AddTournament(AddTournamentRequest request, ServerCallContext context)
        {
            var command = new AddTournamentMongoCommand { Name = request.Name, Description = request.Description };
            var results = await _mediator.Send(command);
            return new Tournament { Name = results.Name, Id = results.Id, Description = results.Description };
        }

        public override async Task<GetTournamentsResponse> GetTournaments(GetTournamentsRequest request, ServerCallContext context)
        {
            var command = new GetTournamentsMongoQuery();
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
            var command = new GetTournamentMongoQuery { Id = request.Id };
            var results = await _mediator.Send(command);

            return new GetTournamentResponse { Tournament = new Tournament { Id = results.Id, Name = results.Name, Description = results.Description} };
        }
    }
}