using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using VirtoCommerce.BackInStock.Data.Repositories;

namespace VirtoCommerce.BackInStock.Data.PostgreSql
{
    public class PostgreSqlDbContextFactory : IDesignTimeDbContextFactory<BackInStockDbContext>
    {
        public BackInStockDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<BackInStockDbContext>();
            var connectionString = args.Any() ? args[0] : "User ID = postgres; Password = password; Host = localhost; Port = 5432; Database = virtocommerce3;";

            builder.UseNpgsql(
                connectionString,
                db => db.MigrationsAssembly(typeof(PostgreSqlDbContextFactory).Assembly.GetName().Name));

            return new BackInStockDbContext(builder.Options);
        }
    }
}
