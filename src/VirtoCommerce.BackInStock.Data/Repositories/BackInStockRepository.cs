using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.BackInStock.Data.Models;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.BackInStock.Data.Repositories;

public class BackInStockRepository(BackInStockDbContext dbContext, IUnitOfWork unitOfWork = null)
    : DbContextRepositoryBase<BackInStockDbContext>(dbContext, unitOfWork), IBackInStockRepository
{
    public IQueryable<BackInStockSubscriptionEntity> BackInStockSubscriptions => DbContext.Set<BackInStockSubscriptionEntity>();

    public virtual async Task<IList<BackInStockSubscriptionEntity>> GetBackInStockSubscriptionsByIdsAsync(IList<string> ids, string responseGroup)
    {
        if (ids.IsNullOrEmpty())
        {
            return [];
        }

        return ids.Count == 1
            ? await BackInStockSubscriptions.Where(x => x.Id == ids.First()).ToListAsync()
            : await BackInStockSubscriptions.Where(x => ids.Contains(x.Id)).ToListAsync();
    }
}
