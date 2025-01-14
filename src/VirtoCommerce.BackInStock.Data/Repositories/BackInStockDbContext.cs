using System.Reflection;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.BackInStock.Data.Repositories;

public class BackInStockDbContext : DbContextBase
{
    public BackInStockDbContext(DbContextOptions<BackInStockDbContext> options)
        : base(options)
    {
    }

    protected BackInStockDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //modelBuilder.Entity<BackInStockEntity>().ToTable("BackInStock").HasKey(x => x.Id);
        //modelBuilder.Entity<BackInStockEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();

        switch (Database.ProviderName)
        {
            case "Pomelo.EntityFrameworkCore.MySql":
                modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.BackInStock.Data.MySql"));
                break;
            case "Npgsql.EntityFrameworkCore.PostgreSQL":
                modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.BackInStock.Data.PostgreSql"));
                break;
            case "Microsoft.EntityFrameworkCore.SqlServer":
                modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.BackInStock.Data.SqlServer"));
                break;
        }
    }
}
