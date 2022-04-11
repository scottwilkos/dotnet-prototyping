using Microsoft.EntityFrameworkCore;
using Prototyping.Domain.Models;
using System.Reflection;

namespace Prototyping.Domain
{
    public class TournamentContext : DbContext
    {
        public TournamentContext(DbContextOptions<TournamentContext> options) : base(options)
        {
        }

        public DbSet<Tournament> Tournaments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tournament>().ToTable("Tournaments");
            modelBuilder.Entity<Tournament>(entity =>
            {
                entity.HasKey(_ => _.Id);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}