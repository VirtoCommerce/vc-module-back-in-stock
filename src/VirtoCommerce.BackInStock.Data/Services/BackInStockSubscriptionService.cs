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

namespace VirtoCommerce.BackInStock.Data.Services
{
    public class BackInStockSubscriptionService :
        CrudService<BackInStockSubscription, BackInStockSubscriptionEntity, BackInStockSubscriptionChangingEvent,
            BackInStockSubscriptionChangedEvent>,
        IBackInStockSubscriptionService
    {
        private readonly Func<IBackInStockSubscriptionRepository> _repositoryFactory;
        private readonly IEventPublisher _eventPublisher;

        public BackInStockSubscriptionService(
            Func<IBackInStockSubscriptionRepository> repositoryFactory,
            IPlatformMemoryCache platformMemoryCache,
            IEventPublisher eventPublisher)
            : base(repositoryFactory, platformMemoryCache, eventPublisher)
        {
            _repositoryFactory = repositoryFactory;
            _eventPublisher = eventPublisher;
        }

        protected override Task<IList<BackInStockSubscriptionEntity>> LoadEntities(IRepository repository,
            IList<string> ids,
            string responseGroup)
        {
            return ((IBackInStockSubscriptionRepository)repository).GetByIdsAsync(ids);
        }
    }
}
