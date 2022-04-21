using MediatR;

namespace Prototyping.Business.Cqrs
{
    public class GetTournamentRecordCountMongoQuery : IRequest<long>{}
}