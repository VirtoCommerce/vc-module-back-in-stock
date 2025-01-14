using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using VirtoCommerce.BackInStock.Data.Repositories;

namespace VirtoCommerce.BackInStock.Data.PostgreSql;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BackInStockDbContext>
{
    public BackInStockDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<BackInStockDbContext>();
        var connectionString = args.Any() ? args[0] : "Server=localhost;Username=virto;Password=virto;Database=VirtoCommerce3;";

        builder.UseNpgsql(
            connectionString,
            options => options.MigrationsAssembly(typeof(PostgreSqlDataAssemblyMarker).Assembly.GetName().Name));

        return new BackInStockDbContext(builder.Options);
    }
}
