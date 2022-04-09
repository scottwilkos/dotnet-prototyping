using System.ComponentModel.DataAnnotations;

namespace Prototyping.Domain.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(500)]
        public string TaskBody { get; set; }
       
        public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
    }
}
