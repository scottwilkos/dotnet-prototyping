using System.ComponentModel.DataAnnotations;

namespace Prototyping.Domain.Models
{
    public class Tournament
    {
        protected Tournament()
        {
        }

        public Tournament(string name, string description)
        {
            Name = name;
            Description = description;
        }

        [Key]
        [Required]
        public string Id { get; protected set; } = Guid.NewGuid().ToString();

        public string Name { get; protected set; }

        public string Description { get; protected set; }
    }
}
