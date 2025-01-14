using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using VirtoCommerce.BackInStock.Data.Repositories;

namespace VirtoCommerce.BackInStock.Data.SqlServer;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BackInStockDbContext>
{
    public BackInStockDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<BackInStockDbContext>();
        var connectionString = args.Length != 0 ? args[0] : "Server=(local);User=virto;Password=virto;Database=VirtoCommerce3;";

        builder.UseSqlServer(
            connectionString,
            options => options.MigrationsAssembly(typeof(SqlServerDataAssemblyMarker).Assembly.GetName().Name));

        return new BackInStockDbContext(builder.Options);
    }
}
