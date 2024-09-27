using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using VirtoCommerce.BackInStockModule.Data.Repositories;

namespace VirtoCommerce.BackInStockModule.Data.MySql;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BackInStockModuleDbContext>
{
    public BackInStockModuleDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<BackInStockModuleDbContext>();
        var connectionString = args.Any() ? args[0] : "Server=localhost;User=virto;Password=virto;Database=VirtoCommerce3;";

        builder.UseMySql(
            connectionString,
            ResolveServerVersion(args, connectionString),
            options => options.MigrationsAssembly(typeof(MySqlDataAssemblyMarker).Assembly.GetName().Name));

        return new BackInStockModuleDbContext(builder.Options);
    }

    private static ServerVersion ResolveServerVersion(string[] args, string connectionString)
    {
        var serverVersion = args.Length >= 2 ? args[1] : null;

        if (serverVersion == "AutoDetect")
        {
            return ServerVersion.AutoDetect(connectionString);
        }

        if (serverVersion != null)
        {
            return ServerVersion.Parse(serverVersion);
        }

        return new MySqlServerVersion(new Version(5, 7));
    }
}
