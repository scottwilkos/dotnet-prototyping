using System.ComponentModel.DataAnnotations;
using MediatR;
using Prototyping.Domain.Models;

namespace Prototyping.Business.Cqrs
{
    public class AddTaskCommand : IRequest<TaskItem>
    {
        [Required]
        [StringLength(500)]
        public string Taskbody { get; internal set; }
    }
}