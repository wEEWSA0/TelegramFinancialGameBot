using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TelegramFinancialGameBot.Data
{
    internal class DbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public static string ConnectionString
            = "User ID=postgres;Password=postgres;Server=localhost;Port=5433;Database=weewsa_financialgamebot_db; IntegratedSecurity=true;Pooling=true;";

        public static DatabaseContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();

            optionsBuilder.UseNpgsql(ConnectionString, (c) => c.EnableRetryOnFailure(
                maxRetryCount: 2,
                maxRetryDelay: TimeSpan.FromSeconds(5),
                errorCodesToAdd: null
                ));

            return new DatabaseContext(optionsBuilder.Options);
        }

        public DatabaseContext CreateDbContext(string[] args)
        {
            return CreateDbContext();
        }
    }
}
