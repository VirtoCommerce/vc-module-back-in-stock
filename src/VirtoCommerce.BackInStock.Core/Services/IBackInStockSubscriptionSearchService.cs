using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.Platform.Core.GenericCrud;

namespace VirtoCommerce.BackInStock.Core.Services
{
    public interface IBackInStockSubscriptionSearchService : ISearchService<BackInStockSubscriptionSearchCriteria,
        BackInStockSubscriptionSearchResult, BackInStockSubscription>
    {
    }
}
