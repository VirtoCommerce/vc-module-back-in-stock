using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.BackInStockModule.Data.Models;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.BackInStockModule.Data.Repositories;

public class BackInStockSubscriptionRepository
    : DbContextRepositoryBase<BackInStockModuleDbContext>, IBackInStockSubscriptionRepository
{
    public BackInStockSubscriptionRepository(BackInStockModuleDbContext moduleDbContext)
        : base(moduleDbContext)
    {
    }

    public IQueryable<BackInStockSubscriptionEntity> BackInStockSubscriptions => DbContext.Set<BackInStockSubscriptionEntity>();

    public async Task<IList<BackInStockSubscriptionEntity>> GetByIdsAsync(IList<string> ids)
    {
        return await BackInStockSubscriptions.Where(x => ids.Contains(x.Id)).ToListAsync();
    }
}
