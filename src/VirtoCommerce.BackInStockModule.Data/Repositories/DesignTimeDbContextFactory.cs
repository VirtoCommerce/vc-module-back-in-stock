using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VirtoCommerce.BackInStockModule.Data.Repositories;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BackInStockModuleDbContext>
{
    public BackInStockModuleDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<BackInStockModuleDbContext>();
        var connectionString = args.Length != 0 ? args[0] : "Server=(local);User=virto;Password=virto;Database=VirtoCommerce3;";

        builder.UseSqlServer(
            connectionString,
            options => options.MigrationsAssembly(typeof(SqlServerDataAssemblyMarker).Assembly.GetName().Name));

        return new BackInStockModuleDbContext(builder.Options);
    }
}
