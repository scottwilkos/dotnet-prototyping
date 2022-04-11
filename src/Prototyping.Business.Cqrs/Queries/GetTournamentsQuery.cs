using MediatR;
using Prototyping.Domain.Models;

namespace Prototyping.Business.Cqrs.Queries
{
    public class GetTournamentsQuery : IRequest<IList<Tournament>> { }
}