using System.Reflection;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.BackInStock.Data.Models;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.BackInStock.Data.Repositories
{
    public class BackInStockDbContext : DbContextBase
    {
#pragma warning disable S109
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

            base.OnModelCreating(modelBuilder);

            // Allows configuration for an entity type for different database types.
            // Applies configuration from all <see cref="IEntityTypeConfiguration{TEntity}" in VirtoCommerce.BackInStock.Data.XXX project. />
            switch (this.Database.ProviderName)
            {
                case "Pomelo.EntityFrameworkCore.MySql":
                    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.BackInStock.Data.MySql"));
                    break;
                case "Npgsql.EntityFrameworkCore.PostgreSQL":
                    modelBuilder.ApplyConfigurationsFromAssembly(
                        Assembly.Load("VirtoCommerce.BackInStock.Data.PostgreSql"));
                    break;
                case "Microsoft.EntityFrameworkCore.SqlServer":
                    modelBuilder.ApplyConfigurationsFromAssembly(
                        Assembly.Load("VirtoCommerce.BackInStock.Data.SqlServer"));
                    break;
            }
        }
#pragma warning restore S109
    }
}
