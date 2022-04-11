using System.ComponentModel.DataAnnotations;
using MediatR;
using Prototyping.Domain.Models;

namespace Prototyping.Business.Cqrs
{
    public class AddTournamentCommand : IRequest<Tournament>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }
    }
}