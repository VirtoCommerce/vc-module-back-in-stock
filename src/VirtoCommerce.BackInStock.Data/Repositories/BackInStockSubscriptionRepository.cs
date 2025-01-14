using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.BackInStock.Data.Models;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.BackInStock.Data.Repositories;

public class BackInStockSubscriptionRepository : DbContextRepositoryBase<BackInStockDbContext>, IBackInStockSubscriptionRepository
{
    public BackInStockSubscriptionRepository(BackInStockDbContext moduleDbContext)
        : base(moduleDbContext)
    {
    }

    public IQueryable<BackInStockSubscriptionEntity> BackInStockSubscriptions => DbContext.Set<BackInStockSubscriptionEntity>();

    public async Task<IList<BackInStockSubscriptionEntity>> GetByIdsAsync(IList<string> ids)
    {
        return await BackInStockSubscriptions.Where(x => ids.Contains(x.Id)).ToListAsync();
    }
}
