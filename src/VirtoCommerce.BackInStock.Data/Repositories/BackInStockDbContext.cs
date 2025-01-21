using System.Reflection;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.BackInStock.Data.Models;
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

        modelBuilder.Entity<BackInStockSubscriptionEntity>().ToTable("BackInStockSubscription").HasKey(x => x.Id);
        modelBuilder.Entity<BackInStockSubscriptionEntity>().Property(x => x.Id).HasMaxLength(IdLength).ValueGeneratedOnAdd();
        modelBuilder.Entity<BackInStockSubscriptionEntity>().HasIndex(x => new { x.UserId, x.ProductId, x.StoreId }).IsUnique();
        modelBuilder.Entity<BackInStockSubscriptionEntity>().HasIndex(x => new { x.MemberId }).IsUnique(false);
        modelBuilder.Entity<BackInStockSubscriptionEntity>().HasIndex(x => new { x.ProductId, x.IsActive }).IsUnique(false);

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
