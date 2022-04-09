using Microsoft.EntityFrameworkCore;
using Prototyping.Domain.Models;

namespace Prototyping.Domain
{
    public class TaskContext : DbContext
    {
        public TaskContext(DbContextOptions<TaskContext> options) : base(options)
        {
        }

        public DbSet<TaskItem> TaskItems { get; set; }

    }
}