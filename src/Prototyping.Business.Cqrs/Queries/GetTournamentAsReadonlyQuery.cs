using System.ComponentModel.DataAnnotations;
using MediatR;
using Prototyping.Domain.Models;

namespace Prototyping.Business.Cqrs.Queries
{
    public class GetTournamentAsReadonlyQuery : IRequest<Tournament>{
        
        [Required]
        public string Id { get; set; } = string.Empty;
    }
}