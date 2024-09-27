using System.Reflection;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.BackInStockModule.Data.Models;
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

        modelBuilder.Entity<BackInStockSubscriptionEntity>().HasKey(x => x.Id);
        modelBuilder.Entity<BackInStockSubscriptionEntity>()
            .ToTable("BackInStockSubscriptions");
        modelBuilder.Entity<BackInStockSubscriptionEntity>().Property(x => x.Id).HasMaxLength(128)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<BackInStockSubscriptionEntity>()
            .HasIndex(x => new { x.StoreId, x.UserId, x.ProductId })
            .IsUnique();
        modelBuilder.Entity<BackInStockSubscriptionEntity>()
            .HasIndex(x => new { x.ProductId })
            .IsUnique(false);

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
