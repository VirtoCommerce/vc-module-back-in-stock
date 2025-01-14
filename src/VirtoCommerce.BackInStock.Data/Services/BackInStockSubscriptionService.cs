using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.BackInStock.Core.Events;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.BackInStock.Core.Services;
using VirtoCommerce.BackInStock.Data.Models;
using VirtoCommerce.BackInStock.Data.Repositories;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Data.GenericCrud;

namespace VirtoCommerce.BackInStock.Data.Services;

public class BackInStockSubscriptionService(
    Func<IBackInStockSubscriptionRepository> repositoryFactory,
    IPlatformMemoryCache platformMemoryCache,
    IEventPublisher eventPublisher)
    : CrudService<BackInStockSubscription, BackInStockSubscriptionEntity, BackInStockSubscriptionChangingEvent, BackInStockSubscriptionChangedEvent>
        (repositoryFactory, platformMemoryCache, eventPublisher),
        IBackInStockSubscriptionService
{
    protected override Task<IList<BackInStockSubscriptionEntity>> LoadEntities(IRepository repository, IList<string> ids, string responseGroup)
    {
        return ((IBackInStockSubscriptionRepository)repository).GetByIdsAsync(ids);
    }
}
