using System.ComponentModel.DataAnnotations;
using MediatR;
using Prototyping.Common.Dtos;

namespace Prototyping.Business.Cqrs
{
    public class AddTournamentMongoCommand : IRequest<ITournament>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }
    }
}