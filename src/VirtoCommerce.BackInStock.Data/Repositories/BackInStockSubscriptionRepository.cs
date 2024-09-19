using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.BackInStock.Data.Models;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.BackInStock.Data.Repositories
{
    public class BackInStockSubscriptionRepository : DbContextRepositoryBase<BackInStockDbContext>,
        IBackInStockSubscriptionRepository
    {
        public BackInStockSubscriptionRepository(BackInStockDbContext dbContext)
            : base(dbContext)
        {
        }

        public IQueryable<BackInStockSubscriptionEntity> BackInStockSubscriptions =>
            DbContext.Set<BackInStockSubscriptionEntity>();

        public async Task<IList<BackInStockSubscriptionEntity>> GetByIdsAsync(IList<string> ids)
        {
            return await BackInStockSubscriptions.Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<IList<BackInStockSubscriptionEntity>> GetActiveSubscriptionsByProductIds(
            IList<string> productIds,
            bool tracking = true)
        {
            var subscriptions = BackInStockSubscriptions.Where(x => productIds.Contains(x.Id));
            if (tracking == false)
            {
                subscriptions = subscriptions.AsNoTracking();
            }

            return await subscriptions.ToListAsync();
        }

        public async Task DeleteBackInStockSubscriptionsAsync(IList<string> ids)
        {
            var items = await GetByIdsAsync(ids);
            DbContext.RemoveRange(items);
        }
    }
}
