using System.Reflection;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.BackInStockModule.Data.Repositories;

public class BackInStockModuleDbContext : DbContextBase
{
    public BackInStockModuleDbContext(DbContextOptions<BackInStockModuleDbContext> options)
        : base(options)
    {
    }

    protected BackInStockModuleDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //modelBuilder.Entity<BackInStockModuleEntity>().ToTable("BackInStockModule").HasKey(x => x.Id);
        //modelBuilder.Entity<BackInStockModuleEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();

        switch (Database.ProviderName)
        {
            case "Pomelo.EntityFrameworkCore.MySql":
                modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.BackInStockModule.Data.MySql"));
                break;
            case "Npgsql.EntityFrameworkCore.PostgreSQL":
                modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.BackInStockModule.Data.PostgreSql"));
                break;
            case "Microsoft.EntityFrameworkCore.SqlServer":
                modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.BackInStockModule.Data.SqlServer"));
                break;
        }
    }
}
