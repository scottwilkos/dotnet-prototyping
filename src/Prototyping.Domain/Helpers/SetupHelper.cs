using Microsoft.EntityFrameworkCore;

namespace Prototyping.Domain.Helpers
{
    public class SetupHelper
    {
        public static void SetupDb(string dbName, string migrationsAssembly){
            if (File.Exists(dbName))
            {
                File.Delete(dbName);
            }

            DbContextOptions<TournamentContext> dbOptions = new DbContextOptionsBuilder<TournamentContext>()
                
                .UseSqlite($"Filename={dbName}", options => { options.MigrationsAssembly(migrationsAssembly); })
                .Options;
            var context = new TournamentContext(dbOptions);
            context.Database.EnsureCreated();
        }
    }
}