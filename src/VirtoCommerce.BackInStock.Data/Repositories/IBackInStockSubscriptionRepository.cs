using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.BackInStock.Data.Models;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.BackInStock.Data.Repositories
{
    public interface IBackInStockSubscriptionRepository : IRepository
    {
        IQueryable<BackInStockSubscriptionEntity> BackInStockSubscriptions { get; }

        Task<IList<BackInStockSubscriptionEntity>> GetByIdsAsync(IList<string> ids);

        Task<IList<BackInStockSubscriptionEntity>> GetActiveSubscriptionsByProductIds(IList<string> productIds,
            bool tracking = true);

        Task DeleteBackInStockSubscriptionsAsync(IList<string> ids);
    }
}
