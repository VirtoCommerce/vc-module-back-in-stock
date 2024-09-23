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
    }
}
