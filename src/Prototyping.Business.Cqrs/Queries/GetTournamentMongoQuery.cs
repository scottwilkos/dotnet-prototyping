using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Prototyping.Business.Cqrs
{
    public class GetTournamentMongoQuery : IRequest<TournamentMongoDto>
    {
        [Required]
        public string Id { get; set; }
    }
}