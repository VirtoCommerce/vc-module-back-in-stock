using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using VirtoCommerce.BackInStock.Data.Repositories;

namespace VirtoCommerce.BackInStock.Data.MySql
{
    public class MySqlDbContextFactory : IDesignTimeDbContextFactory<BackInStockDbContext>
    {
        public BackInStockDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<BackInStockDbContext>();
            var connectionString = args.Any() ? args[0] : "server=localhost;user=root;password=virto;database=VirtoCommerce3;";
            var serverVersion = args.Length >= 2 ? args[1] : null;

            builder.UseMySql(
                connectionString,
                ResolveServerVersion(serverVersion, connectionString),
                db => db
                    .MigrationsAssembly(typeof(MySqlDbContextFactory).Assembly.GetName().Name));

            return new BackInStockDbContext(builder.Options);
        }

        private static ServerVersion ResolveServerVersion(string? serverVersion, string connectionString)
        {
            if (serverVersion == "AutoDetect")
            {
                return ServerVersion.AutoDetect(connectionString);
            }
            else if (serverVersion != null)
            {
                return ServerVersion.Parse(serverVersion);
            }
            return new MySqlServerVersion(new Version(5, 7));
        }
    }
}
