using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using VirtoCommerce.BackInStockModule.Data.Repositories;

namespace VirtoCommerce.BackInStockModule.Data.PostgreSql
{
    public class PostgreSqlDbContextFactory : IDesignTimeDbContextFactory<BackInStockModuleDbContext>
    {
        public BackInStockModuleDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<BackInStockModuleDbContext>();
            var connectionString = args.Any() ? args[0] : "User ID = postgres; Password = password; Host = localhost; Port = 5432; Database = virtocommerce3;";

            builder.UseNpgsql(
                connectionString,
                db => db.MigrationsAssembly(typeof(PostgreSqlDbContextFactory).Assembly.GetName().Name));

            return new BackInStockModuleDbContext(builder.Options);
        }
    }
}
