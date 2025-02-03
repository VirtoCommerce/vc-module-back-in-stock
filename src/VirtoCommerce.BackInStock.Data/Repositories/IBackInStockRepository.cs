using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.BackInStock.Data.Models;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.BackInStock.Data.Repositories;

public interface IBackInStockRepository : IRepository
{
    IQueryable<BackInStockSubscriptionEntity> BackInStockSubscriptions { get; }

    Task<IList<BackInStockSubscriptionEntity>> GetBackInStockSubscriptionsByIdsAsync(IList<string> ids, string responseGroup);
}
