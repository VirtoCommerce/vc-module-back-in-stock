using VirtoCommerce.BackInStockModule.Core.Models;
using VirtoCommerce.Platform.Core.GenericCrud;

namespace VirtoCommerce.BackInStockModule.Core.Services;

public interface IBackInStockSubscriptionSearchService
    : ISearchService<BackInStockSubscriptionSearchCriteria,
        BackInStockSubscriptionSearchResult, BackInStockSubscription>
{
}
