using Microsoft.EntityFrameworkCore;

namespace Prototyping.Domain.Helpers
{
    public class SetupHelper
    {
        public static void SetupDb(string dbName, string migrationsAssembly)
        {
            DbContextOptions<TournamentContext> dbOptions = new DbContextOptionsBuilder<TournamentContext>()

                .UseSqlite($"Filename={dbName}", options => { options.MigrationsAssembly(migrationsAssembly); })
                .Options;
            var context = new TournamentContext(dbOptions);
            context.Database.EnsureCreated();
        }
        
        public static void ClearDb(string dbName, string migrationsAssembly)
        {
            if (File.Exists(dbName))
            {
                File.Delete(dbName);
            }
        }

        public static void EnsureCreated(string dbName, string migrationsAssembly)
        {
            DbContextOptions<TournamentContext> dbOptions = new DbContextOptionsBuilder<TournamentContext>()

                .UseSqlite($"Filename={dbName}", options => { options.MigrationsAssembly(migrationsAssembly); })
                .Options;
            var context = new TournamentContext(dbOptions);
            context.Database.EnsureCreated();
        }

        public static TournamentContext GetDb(string dbName, string migrationsAssembly){
            
            DbContextOptions<TournamentContext> dbOptions = new DbContextOptionsBuilder<TournamentContext>()

                .UseSqlite($"Filename={dbName}", options => { options.MigrationsAssembly(migrationsAssembly); })
                .Options;
            var context = new TournamentContext(dbOptions);
            return context;
        }
    }
}