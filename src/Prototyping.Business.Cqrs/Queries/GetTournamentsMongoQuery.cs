using MediatR;

namespace Prototyping.Business.Cqrs
{
    public class GetTournamentsMongoQuery : IRequest<IList<TournamentMongoDto>>
    {
    }
}