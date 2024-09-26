using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.BackInStockModule.Core.Events;
using VirtoCommerce.BackInStockModule.Core.Models;
using VirtoCommerce.BackInStockModule.Core.Services;
using VirtoCommerce.BackInStockModule.Data.Models;
using VirtoCommerce.BackInStockModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Data.GenericCrud;

namespace VirtoCommerce.BackInStockModule.Data.Services;

public class BackInStockSubscriptionService(
    Func<IBackInStockSubscriptionRepository> repositoryFactory,
    IPlatformMemoryCache platformMemoryCache,
    IEventPublisher eventPublisher)
    :
        CrudService<BackInStockSubscription, BackInStockSubscriptionEntity, BackInStockSubscriptionChangingEvent,
            BackInStockSubscriptionChangedEvent>(repositoryFactory, platformMemoryCache, eventPublisher),
        IBackInStockSubscriptionService
{
    protected override Task<IList<BackInStockSubscriptionEntity>> LoadEntities(IRepository repository, IList<string> ids, string responseGroup)
    {
        return ((IBackInStockSubscriptionRepository)repository).GetByIdsAsync(ids);
    }
}
